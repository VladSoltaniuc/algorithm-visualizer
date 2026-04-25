# Algorithm Visualizer — Deployment Plan

## Project Stack

- **Frontend**: React 19 + TypeScript + Vite (in `/client`)
- **Backend**: ASP.NET Core (.NET 8) API (in `/AlgorithmVisualizer.Api`)
- The backend serves the React app as static files via `UseStaticFiles` + `MapFallbackToFile("index.html")`
- In dev, Vite proxies `/api` → `http://localhost:5286`

## Goal

Deploy to `https://algorithms.vlad-soltaniuc.com/`

## Why Not Cloudflare Pages (like the personal website)

The personal website (`https://www.vlad-soltaniuc.com/`) is pure React (no backend) and is hosted on Cloudflare Workers/Pages.

This project has a real .NET 8 server — Cloudflare Pages cannot run .NET. So it needs a separate hosting service.

## Chosen Architecture

```
algorithms.vlad-soltaniuc.com
        |
   Cloudflare DNS (CNAME record — same Cloudflare account)
        |
   Railway.app (runs the .NET server)
```

Cloudflare is used only for DNS routing. Railway hosts and runs the actual app.

## Deployment Steps

1. **Push repo to GitHub** (if not already public/accessible)
2. **Sign up at railway.app** → connect GitHub account
3. **Create new project** from the GitHub repo → Railway auto-detects .NET and builds it
4. **Note the Railway URL** (e.g. `algo-viz.up.railway.app`)
5. **In Cloudflare DNS dashboard** → add a CNAME record:
   - Name: `algorithms`
   - Target: `algo-viz.up.railway.app`
6. **In Railway** → Settings → Custom Domain → add `algorithms.vlad-soltaniuc.com`

## Code Changes Needed Before Deploying

### A) Auto-build the React app as part of `dotnet publish`

Add a `<Target>` to `AlgorithmVisualizer.Api/AlgorithmVisualizer.Api.csproj` that:
- Runs `npm install` and `npm run build` in `/client`
- Copies `client/dist/` into `AlgorithmVisualizer.Api/wwwroot/`

This way a single `dotnet publish` handles everything.

### B) Verify CORS is not blocking the subdomain (if frontend and API are ever split)

Currently the backend serves the frontend, so CORS is not an issue.

## Notes

- Railway free tier has usage limits but is fine for a portfolio project
- Cloudflare proxies the subdomain the same way it does for the main site
- The `.sln` file is at the repo root: `algorithm-visualizer.sln`
- API project path: `AlgorithmVisualizer.Api/AlgorithmVisualizer.Api.csproj`
- Client path: `client/`
