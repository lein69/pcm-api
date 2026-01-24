import { defineStore } from 'pinia'
import { ref } from 'vue'
import apiClient from '@/api'

export const useDataStore = defineStore('data', () => {
  const members = ref([])
  const courts = ref([])
  const bookings = ref([])
  const challenges = ref([])
  const matches = ref([])
  const transactions = ref([])
  const news = ref([])

  const fetchMembers = async () => {
    try {
      const response = await apiClient.get('/members')
      members.value = response.data
    } catch (error) {
      console.error('Failed to fetch members', error)
    }
  }

  const fetchCourts = async () => {
    try {
      const response = await apiClient.get('/courts')
      courts.value = response.data
    } catch (error) {
      console.error('Failed to fetch courts', error)
    }
  }

  const fetchBookings = async () => {
    try {
      const response = await apiClient.get('/bookings')
      bookings.value = response.data
    } catch (error) {
      console.error('Failed to fetch bookings', error)
    }
  }

  const fetchChallenges = async () => {
    try {
      const response = await apiClient.get('/challenges')
      challenges.value = response.data
    } catch (error) {
      console.error('Failed to fetch challenges', error)
    }
  }

  const fetchMatches = async () => {
    try {
      const response = await apiClient.get('/matches')
      matches.value = response.data
    } catch (error) {
      console.error('Failed to fetch matches', error)
    }
  }

  const fetchTransactions = async () => {
    try {
      const response = await apiClient.get('/transactions')
      transactions.value = response.data
    } catch (error) {
      console.error('Failed to fetch transactions', error)
    }
  }

  const fetchNews = async () => {
    try {
      const response = await apiClient.get('/news')
      news.value = response.data
    } catch (error) {
      console.error('Failed to fetch news', error)
    }
  }

  return {
    members,
    courts,
    bookings,
    challenges,
    matches,
    transactions,
    news,
    fetchMembers,
    fetchCourts,
    fetchBookings,
    fetchChallenges,
    fetchMatches,
    fetchTransactions,
    fetchNews
  }
})