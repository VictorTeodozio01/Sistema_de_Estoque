<template>
    <v-card-title>{{ produto.id ? 'Editar Produto' : 'Adicionar Produto' }}</v-card-title>
    <v-form ref="form" v-model="valid">
      <v-text-field
        v-model="produto.nome"
        label="Nome do Produto"
        :rules="[rules.required]"
        required
      ></v-text-field>
      <v-text-field
        v-model="produto.descricao"
        label="Descrição"
        :rules="[rules.required]"
      ></v-text-field>
      <v-text-field
        v-model="produto.quantidade"
        label="Quantidade"
        type="number"
        :rules="[rules.required, rules.numeric]"
        required
      ></v-text-field>

      <div class="my-4">
        <label for="categoria">Categoria</label>
        <div class="select-wrapper">
          <select
            v-model="produto.categoriaId"
            id="categoria"
            :disabled="loadingCategorias"
            :class="{'loading': loadingCategorias}"
            required
          >
            <option value="" disabled selected>Selecione uma categoria</option>
            <option v-for="categoria in categorias" :key="categoria.id" :value="categoria.id">
              {{ categoria.nome }}
            </option>
          </select>
          <span v-if="loadingCategorias" class="loading-spinner"></span>
          <div v-if="!produto.categoriaId && !valid" class="error-message">
            Categoria é obrigatória.
          </div>
        </div>
      </div>

      <v-btn
        :disabled="!valid || loading || !produto.categoriaId"
        :loading="loading"
        color="primary"
        class="mt-4"
        @click="salvarProduto"
      >
        {{ produto.id ? 'Salvar Alterações' : 'Cadastrar' }}
      </v-btn>
    </v-form>
    <v-card-actions>
      <v-spacer />
      <v-btn text color="red" @click="fechar">Fechar</v-btn>
    </v-card-actions>
</template>

<script>
export default {
  props: {
    produtoInicial: {
      type: Object,
      default: () => ({
        nome: '',
        descricao: '',
        quantidade: 0,
        categoriaId: null,
      }),
    },
  },
  data() {
    return {
      valid: false,
      loading: false,
      produto: { ...this.produtoInicial },
      categorias: [],
      loadingCategorias: false,
      rules: {
        required: (value) => !!value || 'Campo obrigatório.',
        numeric: (value) => !isNaN(value) || 'Deve ser um número.',
      },
    };
  },
  watch: {
    produtoInicial: {
      handler(newVal) {
        this.produto = { ...newVal };
      },
      deep: true,
      immediate: true,
    },
  },
  mounted() {
    this.carregarCategorias();
  },
  methods: {
    async carregarCategorias() {
      this.loadingCategorias = true;
      try {
        const response = await this.$axios.get('/api/categoria');
        this.categorias = response.data.map((c) => ({
          id: c.id,
          nome: c.nome,
        }));
      } catch (error) {
        console.error('Erro ao carregar categorias:', error);
        this.$toast.error('Erro ao carregar categorias.');
      } finally {
        this.loadingCategorias = false;
      }
    },
    async salvarProduto() {
      this.loading = true;
      if (!this.produto.categoriaId) {
        this.valid = false;
        return;
      }
      try {
        const response = this.produto.id
          ? await this.$axios.put(`/api/produto/${this.produto.id}`, this.produto)
          : await this.$axios.post('/api/produto', this.produto);
        this.$emit('produtoSalvo', response.data);
        this.fechar();
      } catch (error) {
        console.error('Erro ao salvar produto:', error);
        this.$toast.error('Erro ao salvar o produto.');
      } finally {
        this.loading = false;
      }
    },
    fechar() {
      this.$emit('fechar');
    },
  },
};
</script>

<style scoped>
.select-wrapper {
  position: relative;
}

select {
  width: 100%;
  padding: 10px 12px;
  font-size: 16px;
  border-radius: 4px;
  border: 1px solid #ccc;
  background-color: #fff;
  appearance: none;
  -webkit-appearance: none;
  -moz-appearance: none;
  cursor: pointer;
}

select:disabled {
  background-color: #f5f5f5;
  cursor: not-allowed;
}

select.loading {
  background-color: #f0f0f0;
}

select:focus {
  outline: none;
  border-color: #1976d2;
}

option {
  padding: 10px;
}

.loading-spinner {
  position: absolute;
  top: 50%;
  right: 15px;
  transform: translateY(-50%);
  width: 16px;
  height: 16px;
  border: 2px solid #ccc;
  border-top: 2px solid #1976d2;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% {
    transform: translateY(-50%) rotate(0deg);
  }
  100% {
    transform: translateY(-50%) rotate(360deg);
  }
}

label {
  display: block;
  margin-bottom: 8px;
  font-size: 14px;
  font-weight: bold;
  color: #333;
}

option:disabled {
  color: #999;
}

.error-message {
  color: red;
  font-size: 12px;
  margin-top: 5px;
}
</style>
