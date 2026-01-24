import { defineStore } from 'pinia'
import { ref } from 'vue'
import axios from 'axios'

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
      const response = await axios.get('http://localhost:7183/api/members')
      members.value = response.data
    } catch (error) {
      console.error('Failed to fetch members')
    }
  }

  const fetchCourts = async () => {
    try {
      const response = await axios.get('http://localhost:7183/api/courts')
      courts.value = response.data
    } catch (error) {
      console.error('Failed to fetch courts')
    }
  }

  const fetchBookings = async () => {
    try {
      const response = await axios.get('http://localhost:7183/api/bookings')
      bookings.value = response.data
    } catch (error) {
      console.error('Failed to fetch bookings')
    }
  }

  const fetchChallenges = async () => {
    try {
      const response = await axios.get('http://localhost:7183/api/challenges')
      challenges.value = response.data
    } catch (error) {
      console.error('Failed to fetch challenges')
    }
  }

  const fetchMatches = async () => {
    try {
      const response = await axios.get('http://localhost:7183/api/matches')
      matches.value = response.data
    } catch (error) {
      console.error('Failed to fetch matches')
    }
  }

  const fetchTransactions = async () => {
    try {
      const response = await axios.get('http://localhost:7183/api/transactions')
      transactions.value = response.data
    } catch (error) {
      console.error('Failed to fetch transactions')
    }
  }

  const fetchNews = async () => {
    try {
      const response = await axios.get('http://localhost:7183/api/news')
      news.value = response.data
    } catch (error) {
      console.error('Failed to fetch news')
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