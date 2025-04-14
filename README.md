# ARSounds

**ARSounds** is a modern, cross-platform application delivering immersive audio experiences through augmented reality, built with cutting-edge .NET technologies. It supports Windows (WPF, WinUI, .NET MAUI), Android, and Web platforms. Designed for modularity, maintainability, and scalability.

---

## ✨ Features

- **Cross-Platform Compatibility:** Fully functional on Windows (WPF, WinUI, .NET MAUI), Android (.NET MAUI), and Web (ASP.NET, Angular).
- **Modular Architecture:** Clean separation of concerns for flexibility and ease of maintenance.
- **Database Flexibility:** Entity Framework Core integration supporting MySQL, PostgreSQL, and SQL Server.
- **Localization:** Built-in multi-language support for global users.
- **Secure Identity:** Authentication and authorization powered by **IdentityServer**, implemented using the **[Skoruba IdentityServer Admin template](https://github.com/skoruba/Duende.IdentityServer.Admin)**.
- **Augmented Audio Experiences:** Integrates AR markers with real-time recognition for immersive sound interactions.

---

## 🔊 ARSounds In Action

### User Flow Overview

1. **Sound Upload:**  
   Users upload audio through the ARSounds Web Client, which dynamically renders a soundwave visualization. Upon activation, an image of this soundwave is generated and sent to OpenVision, linking it to the uploaded sound as a visual AR trigger.

2. **Target Activation:**  
   Markers can be activated or deactivated via the web interface. Only active markers are recognized during scanning.

3. **Scanning the Marker:**  
   Via the ARSounds mobile or desktop app, users authenticate and scan soundwave markers with their camera.

4. **Augmented Playback:**  
   Upon recognizing a valid marker:
   - An animated soundwave is displayed in AR.
   - Corresponding audio is played, offering an immersive sound-visual experience.

5. **Behind the Scenes:**
   - When activated, the marker image is sent to **OpenVision**, where visual features are extracted and stored.
   - During scans, ARSounds sends camera input via WebSocket to OpenVision Server.
   - OpenVision matches the image with stored data and signals the app to trigger audio playback.

> The system delivers real-time, personalized sound design through AR-enhanced visual-audio interaction.

---

## 🔧 Architecture Overview

### API Integration
Both **ARSounds Server** and **OpenVision Server** expose REST and GraphQL APIs for flexible client integration, admin tooling, and external services.

### Clean Architecture Principles
- **Core:** Business logic and domain entities
- **Application:** Use cases and services
- **Infrastructure:** Database and external APIs
- **Presentation:** Platform-specific UIs (Web, Desktop, Mobile)

---

## 🧩 Solution Breakdown

- **ARSounds:** Core logic for all client platforms
- **ARSounds.Web:** Web-facing components:
  - IdentityServer setup
  - OpenVision Server (Computer Vision + WebSocket)
  - OpenVision Web Client
  - ARSounds Server (audio handling)
  - ARSounds Angular Client

> 🧱 **Project Scaffolded Using:** The architecture is bootstrapped using the `OpenVision.Web.Template` NuGet package for a modular, extensible foundation.

> For more on OpenVision, visit the [OpenVision Repository](https://github.com/gachris/OpenVision).

---

## 🚀 Getting Started

### ⚡ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022 with:
  - .NET MAUI workload
  - ASP.NET and Web Development
- Docker Desktop (recommended)
- Node.js (for web clients)
- npm or yarn package manager
- mkcert (for local HTTPS certificates)
- Git (for cloning and managing the repository)
- Android SDK and Emulator (for mobile development)

---

### 🚧 Option 1: Docker Compose (Recommended)

#### Update Hosts File
Add the following to your system's hosts file:
```txt
127.0.0.1 openvision.com www.openvision.com api.openvision.com auth.openvision.com account.openvision.com account-api.openvision.com arsounds.openvision.com
```
- **Linux:** `/etc/hosts`  
- **Windows:** `C:\Windows\System32\drivers\etc\hosts`

#### Certificate Setup

##### Create the Root Certificate

Use [mkcert](https://github.com/FiloSottile/mkcert) to generate local self-signed certificates.

> **Note:** On Windows, `mkcert --install` must be executed under **elevated Administrator** privileges.

```powershell
cd shared/nginx/certs
mkcert --install
copy $env:LOCALAPPDATA\mkcert\rootCA-key.pem ./cacerts.pem
copy $env:LOCALAPPDATA\mkcert\rootCA.pem ./cacerts.crt
```

##### Create Certificates for openvision.com

Generate certificates for `openvision.com` including wildcards for subdomains. This ensures compatibility with the nginx proxy setup.

```powershell
cd shared/nginx/certs
mkcert -cert-file openvision.com.crt -key-file openvision.com.key openvision.com *.openvision.com
mkcert -pkcs12 openvision.com.pfx openvision.com *.openvision.com
```

#### Start Services

```bash
docker-compose build
docker-compose up -d
```

---

### 🌐 Option 2: Aspire AppHost (Cloud-Native Dev)
```bash
cd src/OpenVision.Aspire.AppHost
dotnet run
```

---

### 🔧 Option 3: Manual Startup (Dev/Debug)
Manually start the following services via CLI or Visual Studio:
- `ARSounds.Server`
- `OpenVision.Server`
- `OpenVision.Client`
- `OpenVision.IdentityServer.Admin`
- `OpenVision.IdentityServer.STS.Identity`
- `OpenVision.IdentityServer.Admin.Api`

---

## 📄 Client Library Setup
```bash
cd src/OpenVision.IdentityServer.Admin
npm install

cd src/OpenVision.IdentityServer.STS.Identity
npm install
```

> For advanced identity configuration, refer to the [Skoruba IdentityServer Admin Guide](https://github.com/skoruba/Duende.IdentityServer.Admin).

---

## 🖥️ Platform Hosts

- **WPF Host:** `src/ARSounds.Wpf.Host`
- **WinUI Host:** `src/ARSounds.WinUI.Host`
- **.NET MAUI (Windows & Android):** `src/ARSounds.Maui.Host`

---

## 🛠️ Known Issues

- **OpenVision Client does not run in Linux Docker containers**. Use Aspire AppHost or via manual run.
- **ARSounds Client is incompatible with Linux Docker containers**. Works in Aspire AppHost or via manual run.

---

## 📚 Contributing

We welcome community contributions:
1. Fork the repository
2. Create a feature branch
3. Follow code conventions and commit clearly
4. Submit a detailed pull request

---

## 📃 License

Licensed under the MIT License. See the [LICENSE](LICENSE) file for full details.

