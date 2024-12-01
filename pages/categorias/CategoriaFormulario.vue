<template>
  <v-card-title>{{ categoria.id ? 'Editar Categoria' : 'Adicionar Categoria' }}</v-card-title>
  <v-form ref="form" v-model="valid">
    <v-text-field
      v-model="categoria.nome"
      label="Nome da Categoria"
      :rules="[rules.required]"
      required
    ></v-text-field>
    <v-btn
      :disabled="!valid || loading"
      :loading="loading"
      color="primary"
      class="mt-4"
      @click="salvarCategoria"
    >
      {{ categoria.id ? 'Atualizar' : 'Salvar' }}
    </v-btn>
  </v-form>
  <v-card-actions>
    <v-spacer />
    <v-btn text color="red" @click="fechar">Fechar</v-btn>
  </v-card-actions> 
</template>
<script>
import { useNuxtApp } from '#app';

export default {
  props: {
    categoriaInicial: {
      type: Object,
      default: () => ({ nome: '' }),
    },
  },
  data() {
    return {
      valid: false,
      loading: false,
      categoria: { ...this.categoriaInicial },
      rules: {
        required: (value) => !!value || 'Campo obrigatório.',
      },
    };
  },
  watch: {
    categoriaInicial: {
      handler(newVal) {
        this.categoria = { ...newVal };  
      },
      deep: true,
      immediate: true,  
    },
  },
  methods: {
    async salvarCategoria() {
      if (!this.valid) {
        this.$toast.error('Por favor, preencha todos os campos obrigatórios.');
        return;
      }
      this.loading = true;
      try {
        const nuxtApp = useNuxtApp();
        const axios = nuxtApp.$axios; 

        console.log('Dados a serem salvos:', this.categoria);  

        let response;
        if (this.categoria.id) {
          response = await axios.put(`/api/categoria/${this.categoria.id}`, this.categoria);
        } else {
          response = await axios.post('/api/categoria', this.categoria);
        }
        console.log('Resposta da API:', response);
        this.$emit('categoriaSalva', response.data); 
        this.resetForm(); 
      } catch (error) {
        console.error('Erro ao salvar categoria:', error);
        if (error.response) {
          console.error('Erro na resposta da API:', error.response.data);
          this.$toast.error(`Erro: ${error.response.data.message || 'Erro ao salvar a categoria'}`);
        } else {
          this.$toast.error('Erro ao salvar a categoria.');
        }
      } finally {
        this.loading = false;
      }
    },
    resetForm() {
      this.categoria = { nome: '' }; 
      this.$refs.form.reset();
    },
    fechar() {
      this.$emit('fechar'); 
    },
  },
};
</script>