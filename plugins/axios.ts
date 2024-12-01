import axios from 'axios';

export default defineNuxtPlugin(nuxtApp => {
  const axiosInstance = axios.create({
    baseURL: 'http://localhost:5202', 
    headers: {
      'Content-Type': 'application/json',
    },
  });
  nuxtApp.provide('axios', axiosInstance);
});