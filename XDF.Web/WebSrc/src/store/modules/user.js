import {
  loginByUsername,
  logout,
  getUserInfo
} from '@/api/login'
import {
  getToken,
  setToken,
  removeToken
} from '@/utils/auth'
import Layout from '@/views/layout/Layout'
const _import = require('@/router/_import_' + process.env.NODE_ENV)


function generaMenu(data) {
  let routers = [];
  data.forEach((item) => {
    item.component = Layout
    if (item.children && item.children.length) {
      item.children.forEach((child) => {
        let c = child.component;
        child.component = _import(c)
      })
    }
   
    routers.push(item)
  })
  return routers
}
const user = {
  state: {
    user: '',
    status: '',
    code: '',
    token: getToken(),
    name: '',
    avatar: '',
    introduction: '',
    roles: [],
    setting: {
      articlePlatform: []
    }
  },

  mutations: {
    SET_CODE: (state, code) => {
      state.code = code
    },
    SET_TOKEN: (state, token) => {
      state.token = token
    },
    SET_INTRODUCTION: (state, introduction) => {
      state.introduction = introduction
    },
    SET_SETTING: (state, setting) => {
      state.setting = setting
    },
    SET_STATUS: (state, status) => {
      state.status = status
    },
    SET_NAME: (state, name) => {
      state.name = name
    },
    SET_AVATAR: (state, avatar) => {
      state.avatar = avatar
    },
    SET_ROLES: (state, roles) => {
      state.roles = roles
    },
    SET_MENUS: (state, menus) => {
      state.menus = menus
    }
  },

  actions: {
    // 用户名登录
    LoginByUsername({
      commit
    }, userInfo) {
      const username = userInfo.username.trim()
      return new Promise((resolve, reject) => {
        loginByUsername(username, userInfo.password).then(response => {
          const data = response.data
          commit('SET_TOKEN', data.Msg)
          setToken(response.data.Msg)
          resolve()
        }).catch(error => {
          reject(error)
        })
      })
    },

    // 获取用户信息
    GetUserInfo({
      commit,
      state
    }) {
      return new Promise((resolve, reject) => {
        getUserInfo(state.token).then(response => {
          if (!response.data) { // 由于mockjs 不支持自定义状态码只能这样hack
            reject('error')
          }
          let data = response.data.Data
          let menus = [{
              path: '/error',
              component: "layout",
              redirect: 'noredirect',
              name: 'errorPages',
              meta: {
                title: 'errorPages',
                icon: '404'
              },
              children: [{
                  path: '401',
                  component: 'errorPage/401',
                  name: 'page401',
                  meta: {
                    title: 'page401',
                    noCache: true
                  }
                },
                {
                  path: '404',
                  component: 'errorPage/404',
                  name: 'page404',
                  meta: {
                    title: 'page404',
                    noCache: true
                  }
                }
              ]
            },
            {
              path: '/form',
              component: "layout",
              redirect: 'noredirect',
              name: 'form',
              meta: {
                title: 'form',
                icon: 'form'
              },
              children: [{
                  path: 'create-form',
                  component: 'form/create',
                  name: 'createForm',
                  meta: {
                    title: 'createForm',
                    icon: 'table'
                  }
                },
                {
                  path: 'edit-form',
                  component: 'form/edit',
                  name: 'editForm',
                  meta: {
                    title: 'editForm',
                    icon: 'table'
                  }
                }
              ]
            }
          ]
         let newMenu =  generaMenu(data.menus);
          response.data.menus = newMenu
          commit('SET_ROLES', data.role)
          commit('SET_NAME', data.name)
          commit('SET_AVATAR', "favicon.ico")
          commit('SET_INTRODUCTION', data.introduction)
          commit('SET_MENUS', data.menus)
          resolve(response)
        }).catch(error => {
          reject(error)
        })
      })
    },

    // 第三方验证登录
    // LoginByThirdparty({ commit, state }, code) {
    //   return new Promise((resolve, reject) => {
    //     commit('SET_CODE', code)
    //     loginByThirdparty(state.status, state.email, state.code).then(response => {
    //       commit('SET_TOKEN', response.data.token)
    //       setToken(response.data.token)
    //       resolve()
    //     }).catch(error => {
    //       reject(error)
    //     })
    //   })
    // },

    // 登出
    LogOut({
      commit,
      state
    }) {
      return new Promise((resolve, reject) => {
        logout(state.token).then(() => {
          commit('SET_TOKEN', '')
          commit('SET_ROLES', [])
          removeToken()
          resolve()
        }).catch(error => {
          reject(error)
        })
      })
    },

    // 前端 登出
    FedLogOut({
      commit
    }) {
      return new Promise(resolve => {
        commit('SET_TOKEN', '')
        removeToken()
        resolve()
      })
    },

    // 动态修改权限
    ChangeRoles({
      commit
    }, role) {
      return new Promise(resolve => {
        commit('SET_TOKEN', role)
        setToken(role)
        getUserInfo(role).then(response => {
          const data = response.data
          commit('SET_ROLES', data.roles)
          commit('SET_NAME', data.name)
          commit('SET_AVATAR', data.avatar)
          commit('SET_INTRODUCTION', data.introduction)
          resolve()
        })
      })
    }
  }
}

export default user
