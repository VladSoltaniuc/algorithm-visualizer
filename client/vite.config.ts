import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import http from 'http'

const agent = new http.Agent({
  keepAlive: true,
  keepAliveMsecs: 10_000,
  maxSockets: 20,
})

export default defineConfig({
  plugins: [react()],
  server: {
    port: 3000,
    proxy: {
      '/api': {
        target: 'http://localhost:5286',
        agent,
      },
    },
  },
})
