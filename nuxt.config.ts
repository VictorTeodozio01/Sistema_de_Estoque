// nuxt.config.ts
export default defineNuxtConfig({
  css: ['vuetify/styles'],
  build: {
    transpile: ['vuetify'],
  },
  plugins: ['~/plugins/axios.ts'],  // Caminho correto para o arquivo axios.ts
  
  compatibilityDate: '2024-11-26',
  vite: {
    define: {
      'process.env.DEBUG': false,
    },
  },
});
