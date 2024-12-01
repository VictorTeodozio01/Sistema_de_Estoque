<template>
  <v-app>
    <v-app-bar app color="primary" dark>
      <v-toolbar-title>Sistema de Estoque</v-toolbar-title>
      <v-btn text to="/">Inicio</v-btn>
      <v-btn text to="/produtos">Produtos</v-btn>
      <v-btn text to="/categorias">Categorias</v-btn>
    </v-app-bar>

    <v-main>
      <NuxtPage />
    </v-main>

    <v-footer app>
      <v-container class="text-center">
        <span>© 2024 Sistema de Estoque</span>
      </v-container>
    </v-footer>
  </v-app>
</template>

<script>
export default {};
</script>

<style>
</style>
