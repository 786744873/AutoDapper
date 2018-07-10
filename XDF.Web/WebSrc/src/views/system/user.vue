<template>
  <div class="app-container calendar-list-container">
     <search :whereColArr="WhereColArr" v-on:Submit="SearchSubmit"></search>
    <div style="float:left;">
      <el-button class="filter-item" type="primary" @click="Find(0)">添加</el-button>
    </div>
        <!--列表-->
   <el-table :data="rows.List" @selection-change="selsOperatorChange" v-loading="listLoading" element-loading-text="给我一点时间" border  style="width: 100%">
      <el-table-column type="selection"></el-table-column>
      <el-table-column prop="CityCode" label="城市" width="100"></el-table-column>
      <el-table-column prop="RealName" label="姓名" width="160"></el-table-column>
      <el-table-column prop="LoginName" label="账号" width="120"></el-table-column>
      <el-table-column prop="PostJob" label="职务" width="100"></el-table-column>
      <el-table-column prop="Mobile" label="手机号" width="160"></el-table-column>
      <el-table-column prop="IsLogin" label="允许登录" width="100"></el-table-column>
      <el-table-column label="操作" width="150">
        <template slot-scope="scope">
          <el-button size="small" @click="Find(scope.row.Id)">编辑</el-button>
          <el-button type="danger" size="small" @click="Delete(scope.row.Id)">删除</el-button>
        </template>
      </el-table-column>
    
    </el-table>
    <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="rows.PageIndex" :page-sizes="[20,30, 50,100]" :page-size="rows.PageSize" layout="total, sizes, prev, pager, next, jumper" :total="rows.Count">
      </el-pagination>
    </div>
    <!--新增界面-->
    <el-dialog :title="title" :visible.sync="aeVisible" :close-on-click-modal="false">
      <el-form :model="entity" label-width="120px">
        <el-form-item label="真实姓名">
          <el-input v-model="entity.RealName"></el-input>
        </el-form-item>

        <el-form-item label="登录账号">
          <el-input v-model="entity.LoginName"></el-input>
        </el-form-item>
        <el-form-item label="部门">
          <el-input v-model="entity.Department"></el-input>
        </el-form-item>
        <el-form-item label="职务">
          <el-input v-model="entity.PostJob"></el-input>
        </el-form-item>
        <el-form-item label="手机号">
          <el-input v-model="entity.Mobile"></el-input>
        </el-form-item>
        <el-form-item label="邮箱">
          <el-input v-model="entity.Email"></el-input>
        </el-form-item>
        <el-form-item label="城市">
          <selectCity v-on:SelectCity="SelectCity" :cityCode="entity.CityCode"></selectCity>
        </el-form-item>
        <el-form-item label="是否允许登录">
          <el-radio-group v-model="entity.IsLogin">
            <el-radio class="radio" :label="1">是</el-radio>
            <el-radio class="radio" :label="0">否</el-radio>
          </el-radio-group>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click.native="aeVisible = false">取消</el-button>
        <el-button type="primary" @click.native="Ae(entity.Id)">提交</el-button>
      </div>
    </el-dialog>
</div>
</template>
<script>
import {
  getUserList,
} from '@/api/user'
import search from '../components/search.vue'
export default {
  data() {
    return {
      WhereColArr: [{ value: 'RealName', label: '姓名' },{ value: 'Mobile', label: '手机号' }],
      listLoading: true,
      rows: {
        List: [],
        PageCount: 0,
        Count: 0,
        PageIndex: 1,
        PageNumber: 0,
        PageSize: 20,
        WhereCol: '', // 筛选字段
        WhereCon: '0', // 筛选条件 0:等于 1:包含 2:开始于 3:结束于 4:不包含 5:大于 6:大于等于 7:小于 8:小于等于 9:介于
        WhereVal: '', // 值
        WhereVal1: '' // 值1
      },
      // 编辑界面是否显示
      aeVisible: false,
      // 数据库实体
      entity: {},
      title: '',
      roleRows: {
        List: [],
        PageCount: 0,
        Count: 0,
        PageIndex: 1,
        PageSize: 20,
        WhereCol: '', // 筛选字段
        WhereCon: '0', // 筛选条件
        WhereVal: '', // 值
        WhereVal1: '' // 值1
      },
      sels: [], // 选中的会员
      roles: [], // 已经拥有的角色
      roleVisible: false, // 设置角色界面
      addRoleVisible: false, // 添加角色界面
      roleSels: [] // 选中的角色
    }
  },
  methods: {
    handleSizeChange(val) {
      this.rows.PageSize = val
      this.getList()
    },
    handleCurrentChange(val) {
      this.rows.PageIndex = val
      this.getList()
    },
    selsOperatorChange: function(sels) {
      this.sels = sels
    },
    selsRoleChange: function(sels) {
      this.roleSels = sels
    },
    // 修改增加
    Ae(id) {
      const handler = id > 0 ? updateAgentOperator : addAgentOperator
      handler(this.entity).then(response => {
        this.$notify({
          title: response.data.IsSuccess ? '成功' : '失败',
          message: response.data.Msg,
          type: response.data.IsSuccess ? 'success' : 'error',
          duration: 3000
        })
        if (response.data.IsSuccess) {
          this.aeVisible = false
          this.getList()
        }
      })
    },
    getList() {
      var para = {}
      Object.assign(para, this.rows)
      para.List = []
      getUserList(para).then(response => {
        Object.assign(this.rows, response.data)
        console.log(this.rows)
        this.listLoading = false
      })
    },
    Delete(id) {
      this.$confirm('确认删除该记录吗?', '提示', { type: 'warning' }).then(() => {
        deleteAgentOperator(id).then(response => {
          this.$notify({
            title: response.data.IsSuccess ? '成功' : '失败',
            message: response.data.Msg,
            type: response.data.IsSuccess ? 'success' : 'error',
            duration: 3000
          })
          if (response.data.IsSuccess) {
            this.getList()
          }
        })
      })
    },
    Find(id) {
      if (id > 0) {
        this.title = '修改'
        findAgentOperatorModel(id).then(response => {
          this.entity = response.data
          this.aeVisible = true
        })
      } else {
        this.title = '添加'
        this.entity = { Id: 0, IsLogin: 1 }
        this.aeVisible = true
      }
    },
    getRoleList() {
      // 获取角色列表
      var para = {}
      this.roleRows.WhereCol = 'CityCode'
      this.roleRows.WhereCon = 0
      this.roleRows.WhereVal = this.sels[0].CityCode
      Object.assign(para, this.roleRows)
      para.List = []
      getRoleList(para).then(response => {
        this.roleRows = response.data
        this.addRoleVisible = true
      })
    },
    selectRole() {
      // 选择角色
      this.addRoleVisible = false
      var rolesArr = this.roles
      this.roleSels.forEach(function(item) {
        var isExit = rolesArr.findIndex(function(value) {
          return item.Id === value.Id
        })
        if (isExit < 0) {
          rolesArr.push(item)
        }
      })
    },
    SetRole() {
      if (this.sels.length !== 1) {
        this.$notify.warning({ message: '请选择一条数据！', duration: 1500 })
        return false
      }
      getRolesByUserId(this.sels[0].Id).then(response => {
        this.roles = response.data
        this.roleVisible = true
      })
    },
    SetRoleSubmit() {
      updateUserRole({
        userId: this.sels[0].Id,
        roleList: this.roles
      }).then(response => {
        this.$notify({
          title: response.data.IsSuccess ? '成功' : '失败',
          message: response.data.Msg,
          type: response.data.IsSuccess ? 'success' : 'error',
          duration: 3000
        })
        if (response.data.IsSuccess) {
          this.roleVisible = false
        }
      })
    },
    delRole(id) {
      var index = this.roles.findIndex(function(value) {
        return id === value.Id
      })
      this.roles.splice(index, 1) // 删除元素
    },
    InitPwd() {
      if (this.sels.length !== 1) {
        this.$notify.warning({ message: '请选择一条数据！', duration: 1000 })
        return false
      }
      this.$prompt('请输入新密码', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消'
      }).then(({ value }) => {
        InitPwd(this.sels[0].Id, value).then(response => {
          this.$notify({
            title: response.data.IsSuccess ? '成功' : '失败',
            message: response.data.Msg,
            type: response.data.IsSuccess ? 'success' : 'error',
            duration: 3000
          })
        })
      }).catch(() => {
        this.$message({
          type: 'info',
          message: '取消'
        })
      })
    },
    SearchSubmit(obj) {
      Object.assign(this.rows, obj)
      this.getList()
    },
    SelectCity(id) {
      this.entity.CityCode = id
    }
  },
  created() {
    this.getList()
  },
  components: {
    search
  }
}
</script>
<style scoped>
.editor-container{
  position: relative;
  height: 100%;
}
</style>
