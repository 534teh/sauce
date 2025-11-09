FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /src

# Install Google Chrome Stable and its dependencies for headless mode
RUN apt-get update && apt-get install -y \
    dos2unix \
    software-properties-common \
    wget \
    gnupg \
    ca-certificates \
    procps \
    jq

RUN apt-get install -y \
    libnspr4 \
    libnss3 \
    libdbus-1-3 \
    libatk1.0-0 \
    libatk-bridge2.0-0 \
    libcups2 \
    libdrm2 \
    libgbm1 \
    libasound2 \
    libpango-1.0-0 \
    libx11-6 \
    libx11-xcb1 \
    libxcb1 \
    libxcomposite1 \
    libxcursor1 \
    libxdamage1 \
    libxext6 \
    libxfixes3 \
    libxi6 \
    libxrandr2 \
    libxrender1 \
    libxss1 \
    libxtst6

RUN wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | gpg --dearmor -o /etc/apt/keyrings/google-chrome.gpg \
    && echo "deb [arch=amd64 signed-by=/etc/apt/keyrings/google-chrome.gpg] http://dl.google.com/linux/chrome/deb/ stable main" > /etc/apt/sources.list.d/google-chrome.list \
    && apt-get update \
    && apt-get install -y google-chrome-stable

RUN apt-get install -y firefox-esr

RUN wget -q https://packages.microsoft.com/keys/microsoft.asc -O- | apt-key add - \
    && add-apt-repository "deb [arch=amd64] https://packages.microsoft.com/repos/edge stable main" \
    && apt-get update \
    && apt-get install -y microsoft-edge-stable

RUN rm -rf /var/lib/apt/lists/*

# Install geckodriver
RUN GECKODRIVER_VERSION=$(curl -s "https://api.github.com/repos/mozilla/geckodriver/releases/latest" | jq -r .tag_name) && \
    wget -q "https://github.com/mozilla/geckodriver/releases/download/${GECKODRIVER_VERSION}/geckodriver-${GECKODRIVER_VERSION}-linux64.tar.gz" && \
    tar -zxf geckodriver-${GECKODRIVER_VERSION}-linux64.tar.gz -C /usr/local/bin && \
    rm geckodriver-${GECKODRIVER_VERSION}-linux64.tar.gz

# Install msedgedriver
RUN EDGEDRIVER_VERSION=$(curl -s "https://msedgewebdriverstorage.blob.core.windows.net/edgewebdriver/LATEST_STABLE" | dos2unix | grep -o '[0-9.]*') && \
    wget -q "https://msedgewebdriverstorage.blob.core.windows.net/edgewebdriver/${EDGEDRIVER_VERSION}/edgedriver_linux64.zip" && \
    unzip edgedriver_linux64.zip -d /usr/local/bin && \
    rm edgedriver_linux64.zip


COPY ["./sauce/sauce.csproj", "sauce/"]
RUN dotnet restore "sauce/sauce.csproj"

COPY . .
WORKDIR "/src/sauce"

# Allow for a debug build argument
ARG DEBUG_MODE=false
RUN if [ "$DEBUG_MODE" = "true" ]; then \
    curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l /vsdbg; \
    fi

# Set environment variable to make dotnet test wait for debugger if in debug mode
ENV VSTEST_HOST_DEBUG=${DEBUG_MODE}

# Entrypoint to run tests.
ENTRYPOINT ["dotnet", "test", "--logger", "trx"]