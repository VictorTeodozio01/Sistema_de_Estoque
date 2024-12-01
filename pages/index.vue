<template>
  <v-container>
    <v-card class="mb-5">
      <v-card-title>Bem-vindo ao Sistema de Estoque</v-card-title>
      <v-card-text>
        <p>Navegue pelo menu 'Produto' e 'Categorias' para cadastrar um produto. Primeiro, cadastre uma categoria e, em seguida, um produto.</p>

      </v-card-text>
    </v-card>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue';

const { $axios } = useNuxtApp();  
onMounted(async () => {
  try {
    
  } catch (error) {
    console.error('Erro ao carregar os dados:', error);
  }
});
</script>
