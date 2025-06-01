<template>
  <form @submit.prevent="submit">
    <label>
      Keyword:
      <input v-model="keyword" autocomplete="off" />
    </label>

    <label>
      URL:
      <input v-model="url" autocomplete="off" />
    </label>

    <button type="submit" :disabled="loading">
      {{ loading ? 'Searching...' : 'Search' }}
    </button>

    <ul v-if="validationErrors.length" class="validation">
      <li v-for="(error, index) in validationErrors" :key="index">
        {{ error }}
      </li>
    </ul>
  </form>
</template>

<script setup>
import { ref } from 'vue'
import axios from 'axios'

const emit = defineEmits(['result'])

const keyword = ref('land registry search')
const url = ref('www.infotrack.co.uk')
const loading = defineModel('loading', { required: true })
const validationErrors = ref([])

// Allows protocol or bare domains like "www.domain.com"
function isValidUrl(value) {
  const urlPattern = /^(https?:\/\/)?([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}(\/\S*)?$/
  return urlPattern.test(value.trim())
}

async function submit() {
  validationErrors.value = []

  if (!keyword.value.trim()) {
    validationErrors.value.push('Keyword is required.')
  }

  if (!url.value.trim()) {
    validationErrors.value.push('URL is required.')
  } else if (!isValidUrl(url.value.trim())) {
    validationErrors.value.push('URL must be a valid web address.')
  }

  if (validationErrors.value.length > 0) {
    emit('result', { error: 'Please fix the errors above and try again.' })
    return
  }

  emit('result', { positions: [] }) // Clear result before new request
  loading.value = true

  try {
    const response = await axios.post('https://localhost:7038/api/search', {
      keyword: keyword.value,
      url: url.value
    })

    emit('result', {
      ...response.data,
      keyword: keyword.value,
      url: url.value
    })
  } catch (err) {
    const status = err.response?.status
    const message = err.response?.data?.error || 'Something went wrong.'

    if (status === 429) {
      emit('result', { error: 'Blocked by Google: ' + message })
    } else if (status === 400) {
      emit('result', { error: 'Invalid input: ' + message })
    } else {
      emit('result', { error: message })
    }
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  background: #fff;
  padding: 2rem 2rem 1rem 1rem;
  border-radius: 8px;
  border: 1px solid #ddd;
  align-items: flex-start;
}

label {
  display: flex;
  flex-direction: column;
  font-weight: 500;
  width: 100%;
}

input {
  padding: 0.5rem;
  font-size: 1rem;
  width: 100%;
  height: 2rem;
}

button {
  padding: 0.5rem 1rem;
  height: 2.5rem;
  font-size: 1rem;
  background-color: #007acc;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}
button:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}

.validation {
  color: red;
  margin-top: 0.5rem;
  padding: 0;
  list-style: none;
}

.validation li {
  position: relative;
  padding-left: 1.25rem;
  margin-bottom: 0.25rem;
  text-align: left;
}

.validation li::before {
  content: '•';
  position: absolute;
  left: 0;
  color: red;
}

</style>