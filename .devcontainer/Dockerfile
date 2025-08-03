FROM mcr.microsoft.com/dotnet/sdk:8.0

# Optional tools and locales
RUN apt-get update && apt-get install -y git curl unzip ca-certificates locales \
    && sed -i '/en_US.UTF-8/s/^# //g' /etc/locale.gen && locale-gen \
    && rm -rf /var/lib/apt/lists/*

# Create a non-root user for better Dev Container experience
ARG USERNAME=vscode
ARG USER_UID=1000
ARG USER_GID=$USER_UID
RUN groupadd --gid $USER_GID $USERNAME \
    && useradd -s /bin/bash --uid $USER_UID --gid $USER_GID -m $USERNAME

WORKDIR /workspace
USER $USERNAME
