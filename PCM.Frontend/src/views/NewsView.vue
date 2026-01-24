<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useDataStore } from '../stores/data'
import axios from 'axios'

const dataStore = useDataStore()

interface NewsItem {
  id: number
  title: string
  content: string
  publishedDate: string
  isPinned: boolean
}

onMounted(() => {
  dataStore.fetchNews()
})
</script>

<template>
  <div class="news">
    <div class="page-header">
      <h2>📰 Tin tức & Thông báo</h2>
      <p>Cập nhật hoạt động mới nhất của CLB</p>
    </div>

    <div class="news-grid">
      <div v-for="news in (dataStore.news as NewsItem[])" :key="news.id" class="news-card" :class="{ pinned: news.isPinned }">
        <div class="news-date">{{ new Date(news.publishedDate || Date.now()).toLocaleDateString('vi-VN') }}</div>
        <h3>
          <span v-if="news.isPinned" class="pin-icon">📌</span>
          {{ news.title }}
        </h3>
        <p class="news-content">{{ news.content }}</p>
        <a href="#" class="read-more">Xem chi tiết →</a>
      </div>
    </div>
  </div>
</template>

<style scoped>
.news {
  padding: 20px;
  max-width: 800px;
  margin: 0 auto;
}

.page-header {
  text-align: center;
  margin-bottom: 30px;
}

.news-card {
  background: white;
  border-radius: 12px;
  padding: 25px;
  margin-bottom: 20px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.05);
  border-left: 4px solid #667eea;
}

.news-card.pinned {
  border-left-color: #f59e0b;
  background-color: #fffbeb;
}

.news-date {
  font-size: 12px;
  color: #888;
  margin-bottom: 5px;
}

.news-card h3 {
  margin: 0 0 10px 0;
  color: #2c3e50;
}

.news-content {
  color: #4b5563;
  line-height: 1.5;
}

.read-more {
  display: inline-block;
  margin-top: 10px;
  color: #667eea;
  text-decoration: none;
  font-weight: 500;
}
</style>