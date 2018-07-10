<template>
  <div class="app-container calendar-list-container">
    <div class="filter-container">
      <search :whereColArr="WhereColArr" v-on:Submit="SearchSubmit"></search>
       <el-col :span="12">
      <el-button class="filter-item" type="primary" @click="Find(0)">添加</el-button>
       </el-col>
    </div>
        <!--列表-->
    <el-table :data="rows.List" v-loading="listLoading" element-loading-text="给我一点时间" border fit highlight-current-row style="width: 100%">
      <el-table-column type="selection"></el-table-column>
      <el-table-column prop="KeyConfig" label="键"  width="180"></el-table-column>
      <el-table-column prop="ValueConfig" label="值" :formatter="formatConfig" ></el-table-column>
      <el-table-column prop="Explains" label="说明" width="220"></el-table-column>
      <el-table-column prop="KeyExplains" label="说明" width="220"></el-table-column>
      <el-table-column prop="Status" label="状态" :formatter="statusFormat"  width="100"></el-table-column>
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
        <el-form-item label="键">
          <el-input v-model="entity.KeyConfig"></el-input>
        </el-form-item>
        <el-form-item label="说明">
          <el-input v-model="entity.Explains"></el-input>
        </el-form-item>
        <el-form-item label="说明">
          <el-input v-model="entity.KeyExplains"></el-input>
        </el-form-item>
        <el-form-item label="状态">
          <el-radio-group v-model="entity.Status">
            <el-radio class="radio" :label="1">可用</el-radio>
            <el-radio class="radio" :label="0">禁用</el-radio>
          </el-radio-group>
        </el-form-item>
        <el-form-item label="值">
          <el-input v-model="entity.ValueConfig1" type="textarea" :autosize="{ minRows: 2, maxRows: 4}" v-if="!isShow"></el-input> 
          <div class="editor-container">
            <json-editor ref="jsonEditor" v-model="entity.ValueConfig1" v-if="isShow"></json-editor>
          </div>
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
 getUserList
} from '@/api/user'
import search from '../components/search.vue'
import JsonEditor from '@/components/JsonEditor'
export default {
  data() {
    return {
      WhereColArr: [
        { value: 'Id', label: '' },
        { value: 'KeyConfig', label: '键' },
        { value: 'Status', label: '状态' }
      ],
      listLoading: true,
      rows: {
        List: [],
        PageCount: 0,
        Count: 0,
        PageIndex: 1,
        PageNumber: 0,
        PageSize: 20,
        WhereCol: '', // 筛选字段
        WhereCon: '', // 筛选条件
        WhereVal: '', // 值
        WhereVal1: '' // 值1
      },
      aeVisible: false,
      entity: {},
      title: '',
      isShow: false
    }
  },
  methods: {
    formatConfig(row) {
      if ((row.ValueConfig).indexOf('{') >= 0) {
        return (row.ValueConfig).substring(0, 120)
      }
      return row.ValueConfig
    },
    statusFormat(row) {
      return row.Status === 1 ? '可用' : '禁用'
    },
    handleSizeChange(val) {
      this.rows.PageSize = val
      this.getList()
    },
    handleCurrentChange(val) {
      this.rows.PageIndex = val
      this.getList()
    },
    // 修改增加
    Ae(id) {
      this.entity.ValueConfig = this.entity.ValueConfig1
      const handler = id > 0 ? updateSysConfig : addSysConfig
      handler(this.entity).then(response => {
        this.$notify({
          title: response.data.IsSuccess ? '成功' : '失败',
          message: response.data.Data,
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
        this.rows = response.data
        this.listLoading = false
      })
    },
    Delete(id) {
      this.$confirm('确认删除该记录吗?', '提示', { type: 'warning' }).then(() => {
        deleteSysConfig(id).then(response => {
          if (response.data.IsSuccess) {
            this.$notify({
              title: '成功',
              message: '删除成功',
              type: 'success',
              duration: 2000
            })
            this.getList()
          }
        })
      })
    },
    Find(id) {
      if (id > 0) {
        this.title = '修改'
        this.rows.List.forEach(item => {
          if (item.Id === id) {
            this.entity = item
            this.isShow = false
            if ((this.entity.ValueConfig).indexOf('{') >= 0) {
              this.entity.ValueConfig1 = JSON.parse(this.entity.ValueConfig)
              this.isShow = true
            } else {
              this.entity.ValueConfig1 = this.entity.ValueConfig
            }
            this.aeVisible = true
          }
          return
        })
      } else {
        this.title = '添加'
        this.entity = { Id: 0, Status: 1, ValueConfig1: '' }
        this.aeVisible = true
      }
    },
    SearchSubmit(obj) {
      Object.assign(this.rows, obj)
      this.getList()
    }
  },
  created() {
    this.getList()
  },
  components: {
    search, JsonEditor
  }
}
</script>

<style scoped>
.editor-container{
  position: relative;
  height: 100%;
}
</style>
