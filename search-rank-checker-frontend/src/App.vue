<template>
  <main class="layout">
    <!-- Main content -->
    <section class="main-panel">
      <div class="container">
        <h1>Search Rank Checker</h1>

        <div class="form-wrapper">
          <SearchForm @result="handleResult" :loading="loading" />
        </div>

        <div v-if="error" class="error">
          {{ error }}
        </div>

        <div v-else-if="positions.length" class="result-box">
          <h2>Search Result</h2>
          <p><strong>Keyword:</strong> {{ lastKeyword }}</p>
          <p><strong>URL:</strong> {{ lastUrl }}</p>
          <p><strong>Matches Found:</strong> {{ positions[0] === 0 ? 0 : positions.length }}</p>

          <div v-if="positions[0] === 0">
            URL not found in top 100 search results.
          </div>
          <p v-else>
            Found at position{{ positions.length > 1 ? 's' : '' }} {{ positions.join(', ') }}
          </p>
        </div>
      </div>
    </section>

    <!-- Simulation panel -->
    <aside class="side-panel">
      <div class="sim-card">
        <h3>Simulate Result</h3>
        <button @click="simulateSuccess" class="simulate-btn">
          Preview Example Result
        </button>
      </div>
    </aside>
  </main>
</template>

<script setup>
import { ref } from 'vue'
import SearchForm from './components/SearchForm.vue'

const positions = ref([])
const error = ref('')
const loading = ref(false)
const lastKeyword = ref('')
const lastUrl = ref('')

function handleResult(result) {
  if (result.error) {
    error.value = result.error
    positions.value = []
  } else {
    positions.value = result.positions
    error.value = ''
    lastKeyword.value = result.keyword ?? lastKeyword.value
    lastUrl.value = result.url ?? lastUrl.value
  }
}

function simulateSuccess() {
  handleResult({
    positions: [1, 8, 17],
    keyword: 'land registry search',
    url: 'www.infotrack.co.uk'
  })
}
</script>

<style scoped>
.layout {
  display: flex;
  justify-content: center;
  align-items: flex-start;
  gap: 2rem;
  max-width: 1200px;
  margin: 3rem auto;
  font-family: 'Segoe UI', sans-serif;
  padding: 0 1rem;
  box-sizing: border-box;
}

/* Main card container */
.container {
  background: #fff;
  border: 1px solid #ddd;
  padding: 2rem;
  padding-bottom: 1.5rem;
  border-radius: 12px;
  width: 500px;
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.05);
  display: flex;
  flex-direction: column;
  box-sizing: border-box;
}

h1 {
  text-align: center;
  margin-bottom: 1.5rem;
  font-size: 1.5rem;
  color: #333;
}

.form-wrapper {
  margin-bottom: 1.5rem;
}

/* Side panel for simulate button */
.side-panel {
  width: 230px;
  flex-shrink: 0;
}

.sim-card {
  background: #f9f9f9;
  border: 1px solid #ccc;
  border-radius: 10px;
  padding: 1rem;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.04);
  text-align: center;
  position: sticky;
  top: 2rem;
}

.sim-card h3 {
  margin-bottom: 1rem;
  font-size: 1.1rem;
  color: #555;
}

.simulate-btn {
  padding: 0.5rem 1rem;
  font-size: 0.95rem;
  background-color: #e8e8e8;
  border: 1px solid #bbb;
  border-radius: 5px;
  cursor: pointer;
  min-width: 180px;
  white-space: nowrap;
}

.simulate-btn:hover {
  background-color: #dedede;
}

/* Error and result display */
.error {
  color: red;
  margin-top: 1rem;
  text-align: center;
}

.result-box {
  background: #f3f9ff;
  border-left: 4px solid #007acc;
  padding: 1rem;
  border-radius: 8px;
  margin-top: 1rem;
}

.result-box h2 {
  margin-bottom: 1rem;
  color: #007acc;
}
</style>
