import request from '@/utils/request'

export function loginByUsername(username, password) {
  const data = {
    username,
    password
  }
  return request({
    url: 'api/user/login',
    method: 'post',
    data
  })
}

export function logout() {
  return request({
    url: 'api/login/logout',
    method: 'post'
  })
}

export function getUserInfo(token) {
  return request({
    url: 'api/user/getUserInfo',
    method: 'get',
    params: { token }
  })
}

