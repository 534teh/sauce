# Base stage for both debug and release
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base
WORKDIR /src

# Install all browsers, dependencies, and utilities in a single layer
RUN apt-get update && apt-get install -y \
    # Utilities
    wget \
    gnupg \
    ca-certificates \
    procps \
    curl \
    jq \
    dos2unix \
    unzip \
    gosu \
    software-properties-common \
    # Browser dependencies
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
    libxtst6 \
  # Add Google Chrome repository (secure method)
  && wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | gpg --dearmor -o /etc/apt/keyrings/google-chrome.gpg \
  && echo "deb [arch=amd64 signed-by=/etc/apt/keyrings/google-chrome.gpg] http://dl.google.com/linux/chrome/deb/ stable main" > /etc/apt/sources.list.d/google-chrome.list \
  # Add Microsoft Edge repository (secure method)
  && wget -q https://packages.microsoft.com/keys/microsoft.asc -O- | gpg --dearmor -o /etc/apt/keyrings/microsoft.gpg \
  && echo "deb [arch=amd64 signed-by=/etc/apt/keyrings/microsoft.gpg] https://packages.microsoft.com/repos/edge stable main" > /etc/apt/sources.list.d/microsoft-edge.list \
  # Update apt list *after* adding all repos
  && apt-get update
  
# Install all browsers
RUN apt-get install -y \
    google-chrome-stable \
    microsoft-edge-stable \
    firefox-esr \
  # Clean up apt cache
  && rm -rf /var/lib/apt/lists/*

# Install webdrivers in separate layers for better caching

# Install geckodriver (for Firefox)
RUN \
  GECKODRIVER_VERSION=$(curl -s "https://api.github.com/repos/mozilla/geckodriver/releases/latest" | jq -r .tag_name) && \
  echo "GECKODRIVER_VERSION=${GECKODRIVER_VERSION}" && \
  curl -L -o geckodriver.tar.gz "https://github.com/mozilla/geckodriver/releases/download/${GECKODRIVER_VERSION}/geckodriver-${GECKODRIVER_VERSION}-linux64.tar.gz" && \
  tar -zxf geckodriver.tar.gz -C /usr/local/bin && \
  chmod +x /usr/local/bin/geckodriver && \
  rm geckodriver.tar.gz

# Install msedgedriver (for Edge)
RUN \
  set -e && \
  EDGE_VERSION=$(microsoft-edge-stable --version | cut -d ' ' -f 3) && \
  if [ -z "$EDGE_VERSION" ]; then \
    echo "ERROR: Failed to retrieve microsoft-edge-stable version." >&2; \
    exit 1; \
  fi && \
  echo "Installing msedgedriver version matching Edge: ${EDGE_VERSION}" && \
  wget -q "https://msedgewebdriverstorage.blob.core.windows.net/edgewebdriver/${EDGE_VERSION}/edgedriver_linux64.zip" && \
  unzip edgedriver_linux64.zip -d /usr/local/bin && \
  chmod +x /usr/local/bin/msedgedriver && \
  rm edgedriver_linux64.zip

# Install chromedriver (for Chrome)
RUN \
  DOWNLOAD_URL=$(curl -s "https://googlechromelabs.github.io/chrome-for-testing/last-known-good-versions-with-downloads.json" | \
  jq -r '.channels.Stable.downloads.chromedriver[] | select(.platform == "linux64").url') && \
  wget -q "${DOWNLOAD_URL}" -O chromedriver.zip && \
  unzip -j chromedriver.zip "chromedriver-linux64/chromedriver" -d /usr/local/bin && \
  chmod +x /usr/local/bin/chromedriver && \
  rm chromedriver.zip

# Copy and restore project file first for caching
COPY ["sauce/sauce.csproj", "sauce/"]
RUN dotnet restore "sauce/sauce.csproj"

# Copy the rest of the project source
COPY ["sauce/", "sauce/"]
COPY entrypoint.sh /usr/local/bin/entrypoint.sh
RUN chmod +x /usr/local/bin/entrypoint.sh
WORKDIR "/src/sauce"

# Debug stage
FROM base AS debug
RUN echo "Installing VS Debugger..." && \
    curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l /vsdbg
ENTRYPOINT ["/bin/sh", "-c", "sleep infinity"]

# Release stage
FROM base AS release
ENTRYPOINT ["/usr/local/bin/entrypoint.sh"]
CMD ["dotnet", "test", "--logger", "trx", "--settings", "NoDeployment.runsettings"]
