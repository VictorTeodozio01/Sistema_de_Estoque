<template>
  <v-container fluid>
    <v-card>
      <v-card-title class="headline">Lista de Categorias</v-card-title>
      <v-row align="center" justify="space-between">
        <v-col cols="12" sm="6" md="4" class="d-flex justify-center">
          <v-btn color="primary" @click="abrirModal" rounded>
            <v-icon left>mdi-plus-circle</v-icon> Adicionar Categoria
          </v-btn>
        </v-col>
        <v-col cols="12" sm="6" md="8">
          <v-text-field
            v-model="filtro"
            label="Filtrar categorias..."
            outlined
            dense
            append-icon="mdi-magnify"
          />
        </v-col>
      </v-row>
    </v-card>
    <v-card class="mt-4">
      <v-simple-table v-if="categorias.length" class="custom-table">
        <thead>
          <tr>
            <th class="th-nome">Nome</th>
            <th class="th-acoes">Ações</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="categoria in categoriasFiltradas" :key="categoria.id">
            <td>{{ categoria.nome }}</td>
            <td>
              <v-row no-gutters>
                <v-col cols="6" class="d-flex justify-center">
                  <v-btn small color="yellow" @click="editarCategoria(categoria)">
                    Editar
                  </v-btn>
                </v-col>
                <v-col cols="6" class="d-flex justify-center">
                  <v-btn small color="red" @click="confirmarExcluirCategoria(categoria)">
                    Excluir
                  </v-btn>
                </v-col>
              </v-row>
            </td>
          </tr>
        </tbody>
      </v-simple-table>
      <v-alert v-else type="info" class="mt-4 white--text" outlined color="white">
        Nenhuma categoria cadastrada.
      </v-alert>
    </v-card>
    <v-dialog v-model="modalCadastro" max-width="500px">
      <v-card>
        <v-card-text>
          <Cadastrar
            :categoriaInicial="categoriaSelecionada"
            @categoriaSalva="atualizarCategoria"
            @fechar="fecharModal"
          />
        </v-card-text>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue';
import Cadastrar from './CategoriaFormulario.vue';

const { $axios } = useNuxtApp();

const categorias = ref([]);
const filtro = ref('');
const categoriaSelecionada = ref(null);
const modalCadastro = ref(false);

onMounted(() => carregarCategorias());

const carregarCategorias = async () => {
  try {
    const response = await $axios.get('/api/categoria');
    categorias.value = Array.isArray(response.data) ? response.data : [];
  } catch (error) {
    console.error('Erro ao carregar categorias:', error);
  }
};

const categoriasFiltradas = computed(() => {
  if (!filtro.value) return categorias.value;
  return categorias.value.filter((categoria) =>
    categoria.nome.toLowerCase().includes(filtro.value.toLowerCase())
  );
});

const abrirModal = () => {
  categoriaSelecionada.value = null;
  modalCadastro.value = true;
};

const editarCategoria = (categoria) => {
  categoriaSelecionada.value = categoria;
  modalCadastro.value = true;
};

const fecharModal = () => {
  modalCadastro.value = false;
  carregarCategorias();
};

const atualizarCategoria = (categoria) => {
  if (categoriaSelecionada.value) {
    const index = categorias.value.findIndex((c) => c.id === categoria.id);
    if (index !== -1) categorias.value[index] = categoria;
  } else {
    categorias.value.push(categoria);
  }
  fecharModal();
};

const confirmarExcluirCategoria = async (categoria) => {
  if (confirm('Tem certeza que deseja excluir esta categoria?')) {
    try {
      await $axios.delete(`/api/categoria/${categoria.id}`);
      categorias.value = categorias.value.filter((c) => c.id !== categoria.id);
    } catch (error) {
      console.error('Erro ao excluir categoria:', error);
    }
  }
};
</script>

<style scoped>
.v-btn {
  font-weight: bold;
}

.v-alert {
  margin-top: 20px;
}

.v-btn small {
  margin-right: 8px;
}

.custom-table th, .custom-table td {
  padding: 16px;
  text-align: left;
  border-bottom: 1px solid #e0e0e0;
  vertical-align: middle;
}

.custom-table th {
  background-color: #f5f5f5;
  font-weight: bold;
}

.custom-table tbody tr:nth-child(even) {
  background-color: #fafafa;
}

.custom-table tbody tr:hover {
  background-color: #f1f1f1;
}

.custom-table .th-nome {
  width: 80%;
}

.custom-table .th-acoes {
  width: 20%;
}

.custom-table td {
  word-wrap: break-word;
}

.v-card .custom-table {
  width: 100%;
  table-layout: fixed;
}

.custom-table .v-btn {
  font-size: 14px;
  margin-right: 8px;
}

@media (max-width: 600px) {
  .custom-table th, .custom-table td {
    padding: 12px 8px;
  }

  .custom-table .th-nome,
  .custom-table .th-acoes {
    font-size: 12px;
  }

  .v-card-title {
    font-size: 18px;
  }
  
  .custom-table .v-row {
    flex-direction: column;
  }

  .custom-table .v-col {
    width: 100%;
    margin-bottom: 8px;
  }
}
</style>
