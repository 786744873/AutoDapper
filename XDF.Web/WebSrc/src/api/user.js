import request from '@/utils/request'

export function getUserList(query) {
  return request({
    url: 'api/user',
    method: 'get',
    params: query
  })
}
export function getEmployeeList(query) {
  return request({
    url: 'api/employee',
    method: 'get',
    params: query
  })
}
