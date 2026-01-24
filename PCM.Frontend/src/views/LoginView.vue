<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const authStore = useAuthStore()

const email = ref('')
const password = ref('')
const message = ref('')
const isLoading = ref(false)

const login = async () => {
  isLoading.value = true
  message.value = ''
  
  const success = await authStore.login(email.value, password.value)
  if (success) {
    router.push('/')
  } else {
    message.value = 'Đăng nhập thất bại! Vui lòng kiểm tra thông tin.'
  }
  isLoading.value = false
}

const quickLogin = async (username: string, pwd: string) => {
  isLoading.value = true
  message.value = ''
  
  const success = await authStore.login(username, pwd)
  if (success) {
    router.push('/')
  } else {
    message.value = 'Đăng nhập thất bại!'
  }
  isLoading.value = false
}
</script>

<template>
  <div class="login-container">
    <div class="login-card">
      <div class="login-header">
        <h2>🎾 PCM Pickleball Club</h2>
        <p>Đăng nhập vào hệ thống quản lý</p>
      </div>

      <form @submit.prevent="login" class="login-form">
        <div class="form-group">
          <label for="email">Tên đăng nhập hoặc Email</label>
          <input 
            id="email"
            v-model="email" 
            type="text" 
            placeholder="Nhập username hoặc email"
            required 
            class="form-input"
          />
        </div>
        
        <div class="form-group">
          <label for="password">Mật khẩu</label>
          <input 
            id="password"
            v-model="password" 
            type="password" 
            placeholder="Nhập mật khẩu"
            required 
            class="form-input"
          />
        </div>

        <button type="submit" class="login-btn" :disabled="isLoading">
          <span v-if="isLoading">Đang đăng nhập...</span>
          <span v-else>Đăng nhập</span>
        </button>
      </form>

      <div v-if="message" class="alert alert-error">
        {{ message }}
      </div>

      <div class="divider">
        <span>hoặc đăng nhập nhanh</span>
      </div>

      <div class="quick-login">
        <h3>Tài khoản demo:</h3>
        
        <button @click="quickLogin('admin', 'Admin123!')" class="quick-btn admin-btn" :disabled="isLoading">
          <div class="btn-content">
            <span class="icon">👑</span>
            <div class="btn-text">
              <strong>Admin</strong>
              <small>Quyền quản trị viên</small>
            </div>
          </div>
        </button>

        <button @click="quickLogin('member1', 'Member123!')" class="quick-btn member-btn" :disabled="isLoading">
          <div class="btn-content">
            <span class="icon">🎾</span>
            <div class="btn-text">
              <strong>Member 1</strong>
              <small>member1 / Member123!</small>
            </div>
          </div>
        </button>

        <button @click="quickLogin('member2', 'Member123!')" class="quick-btn member-btn" :disabled="isLoading">
          <div class="btn-content">
            <span class="icon">🎾</span>
            <div class="btn-text">
              <strong>Member 2</strong>
              <small>member2 / Member123!</small>
            </div>
          </div>
        </button>

        <button @click="quickLogin('member3', 'Member123!')" class="quick-btn member-btn" :disabled="isLoading">
          <div class="btn-content">
            <span class="icon">🎾</span>
            <div class="btn-text">
              <strong>Member 3</strong>
              <small>member3 / Member123!</small>
            </div>
          </div>
        </button>
      </div>

      <div class="login-footer">
        <p>🎯 Hệ thống quản lý sân Pickleball</p>
        <p>📚 Bài tập Full-Stack Development</p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.login-container {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  padding: 20px;
}

.login-card {
  background: white;
  border-radius: 15px;
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1);
  padding: 40px;
  width: 100%;
  max-width: 500px;
  animation: slideUp 0.5s ease-out;
}

@keyframes slideUp {
  from {
    opacity: 0;
    transform: translateY(30px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.login-header {
  text-align: center;
  margin-bottom: 30px;
}

.login-header h2 {
  color: #333;
  margin-bottom: 10px;
  font-size: 28px;
  font-weight: 700;
}

.login-header p {
  color: #666;
  margin: 0;
  font-size: 16px;
}

.login-form {
  margin-bottom: 20px;
}

.form-group {
  margin-bottom: 20px;
}

.form-group label {
  display: block;
  margin-bottom: 8px;
  color: #333;
  font-weight: 500;
  font-size: 14px;
}

.form-input {
  width: 100%;
  padding: 15px;
  border: 2px solid #e1e5e9;
  border-radius: 8px;
  font-size: 16px;
  transition: border-color 0.3s ease;
  box-sizing: border-box;
}

.form-input:focus {
  outline: none;
  border-color: #667eea;
  box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
}

.login-btn {
  width: 100%;
  padding: 15px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 16px;
  font-weight: 600;
  cursor: pointer;
  transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.login-btn:hover:not(:disabled) {
  transform: translateY(-2px);
  box-shadow: 0 10px 20px rgba(102, 126, 234, 0.3);
}

.login-btn:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.alert {
  padding: 12px 16px;
  border-radius: 6px;
  margin-top: 15px;
  font-size: 14px;
}

.alert-error {
  background-color: #fee;
  color: #c33;
  border: 1px solid #fcc;
}

.divider {
  margin: 30px 0;
  text-align: center;
  position: relative;
}

.divider::before {
  content: '';
  position: absolute;
  top: 50%;
  left: 0;
  right: 0;
  height: 1px;
  background: #e1e5e9;
}

.divider span {
  background: white;
  padding: 0 15px;
  color: #666;
  font-size: 14px;
  position: relative;
  z-index: 1;
}

.quick-login {
  margin-bottom: 30px;
}

.quick-login h3 {
  text-align: center;
  color: #333;
  margin-bottom: 20px;
  font-size: 18px;
  font-weight: 600;
}

.quick-btn {
  width: 100%;
  padding: 15px;
  margin-bottom: 10px;
  border: 2px solid #e1e5e9;
  border-radius: 8px;
  background: white;
  cursor: pointer;
  transition: all 0.3s ease;
  text-align: left;
}

.quick-btn:hover:not(:disabled) {
  border-color: #667eea;
  box-shadow: 0 5px 15px rgba(102, 126, 234, 0.2);
  transform: translateY(-1px);
}

.quick-btn:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.btn-content {
  display: flex;
  align-items: center;
  gap: 15px;
}

.icon {
  font-size: 24px;
  width: 40px;
  text-align: center;
}

.btn-text {
  flex: 1;
}

.btn-text strong {
  display: block;
  font-size: 16px;
  color: #333;
  margin-bottom: 2px;
}

.btn-text small {
  color: #666;
  font-size: 12px;
}

.admin-btn {
  border-color: #ffd700;
  background: linear-gradient(135deg, #fff8dc 0%, #f5f5dc 100%);
}

.admin-btn:hover:not(:disabled) {
  border-color: #ffd700;
  box-shadow: 0 5px 15px rgba(255, 215, 0, 0.3);
}

.member-btn {
  border-color: #4CAF50;
  background: linear-gradient(135deg, #f1f8e9 0%, #e8f5e8 100%);
}

.member-btn:hover:not(:disabled) {
  border-color: #4CAF50;
  box-shadow: 0 5px 15px rgba(76, 175, 80, 0.3);
}

.login-footer {
  text-align: center;
  margin-top: 30px;
  padding-top: 20px;
  border-top: 1px solid #e1e5e9;
}

.login-footer p {
  margin: 5px 0;
  color: #666;
  font-size: 14px;
}

@media (max-width: 600px) {
  .login-card {
    padding: 20px;
    margin: 10px;
  }
  
  .login-header h2 {
    font-size: 24px;
  }
  
  .form-input {
    padding: 12px;
  }
  
  .login-btn, .quick-btn {
    padding: 12px;
  }
}
</style>