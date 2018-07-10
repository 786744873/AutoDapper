<template>
  <div class="app-container calendar-list-container">
     <search :whereColArr="WhereColArr" v-on:Submit="SearchSubmit"></search>
    <div style="float:left;">
      <el-button class="filter-item" type="primary" @click="Find(0)">添加</el-button>
    </div>
        <!--列表-->
   <el-table :data="rows.List" @selection-change="selsOperatorChange" v-loading="listLoading" element-loading-text="给我一点时间" border  style="width: 100%">
      <el-table-column type="selection"></el-table-column>
      <el-table-column prop="Id" label="ID"></el-table-column>
      <el-table-column prop="NSchoolId" label="NSchoolId"></el-table-column>
      <el-table-column prop="SName" label="SName"></el-table-column>
      <el-table-column prop="SCode" label="SCode"></el-table-column>
      <el-table-column prop="SNameSpell" label="SNameSpell"></el-table-column>
      <el-table-column prop="NGender" label="NGender" ></el-table-column>
      <el-table-column prop="SPhone" label="SPhone"></el-table-column>
      <el-table-column label="操作" width="150">
        <template slot-scope="scope">
           <router-link :to="'/xdf/employeeEdit/'+scope.row.Id">
            <el-button type="primary" size="small">编辑</el-button>
          </router-link>
          <el-button type="danger" size="small" @click="Delete(scope.row.Id)">删除</el-button>
        </template>
      </el-table-column>
    
    </el-table>
    <div v-show="!listLoading" class="pagination-container">
      <el-pagination @size-change="handleSizeChange" @current-change="handleCurrentChange" :current-page.sync="rows.PageIndex" :page-sizes="[20,30, 50,100]" :page-size="rows.PageSize" layout="total, sizes, prev, pager, next, jumper" :total="rows.Count">
      </el-pagination>
    </div>
</div>
</template>
<script>
import {
  getEmployeeList,
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
      getEmployeeList(para).then(response => {
        Object.assign(this.rows, response.data)
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
      alert(id)
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
