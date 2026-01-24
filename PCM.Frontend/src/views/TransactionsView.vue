<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useDataStore } from '../stores/data'
import axios from 'axios'

const dataStore = useDataStore()

interface Transaction {
  id: number
  date: string
  type: string
  category: string
  description: string
  amount: number
}

onMounted(() => {
  dataStore.fetchTransactions()
})
</script>

<template>
  <div class="transactions">
    <div class="page-header">
      <h2>💰 Quản lý Tài chính</h2>
      <p>Theo dõi dòng tiền Thu/Chi của CLB</p>
    </div>

    <div class="card">
      <table class="table">
        <thead>
          <tr>
            <th>Ngày</th>
            <th>Danh mục</th>
            <th>Mô tả</th>
            <th class="text-right">Số tiền</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="transaction in (dataStore.transactions as Transaction[])" :key="transaction.id">
            <td>{{ new Date(transaction.date).toLocaleDateString('vi-VN') }}</td>
            <td>
              <span class="badge" :class="transaction.type === 'Thu' ? 'income' : 'expense'">
                {{ transaction.category }}
              </span>
            </td>
            <td>{{ transaction.description || 'Không có mô tả' }}</td>
            <td class="text-right amount" :class="transaction.type === 'Thu' ? 'text-income' : 'text-expense'">
              {{ transaction.type === 'Thu' ? '+' : '-' }}
              {{ new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(transaction.amount) }}
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<style scoped>
.transactions {
  padding: 20px;
  max-width: 1000px;
  margin: 0 auto;
}

.page-header {
  text-align: center;
  margin-bottom: 30px;
}

.card {
  background: white;
  border-radius: 12px;
  box-shadow: 0 4px 6px rgba(0,0,0,0.1);
  overflow: hidden;
}

.table {
  width: 100%;
  border-collapse: collapse;
}

th, td {
  padding: 15px;
  text-align: left;
  border-bottom: 1px solid #eee;
}

.text-right { text-align: right; }

.badge {
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: bold;
}

.badge.income { background: #dcfce7; color: #166534; }
.badge.expense { background: #fee2e2; color: #991b1b; }

.amount { font-weight: bold; }
.text-income { color: #166534; }
.text-expense { color: #dc2626; }
</style>