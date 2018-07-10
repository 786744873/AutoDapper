<template>

<div style="float:left;">
<el-form :inline="true">
  <el-form-item>
     <el-select v-model="whereColValue" clearable style="width: 120px" class="filter-item">
        <el-option v-for="item in whereColArr" sele :key="item.value" :label="item.label" :value="item.value"> </el-option>
    </el-select>
  </el-form-item>
  <el-form-item>
    <el-select v-model="whereCon" placeholder="请选择" clearable style="width: 150px;margin-left:9px" class="filter-item">
        <el-option label="等于" value="0"></el-option>
        <el-option label="包含" value="1"></el-option>
        <el-option label="开始于" value="2"></el-option>
        <el-option label="结束于" value="3"></el-option>
        <el-option label="不包含" value="4"></el-option>
        <el-option label="大于" value="5"></el-option>
        <el-option label="大于等于" value="6"></el-option>
        <el-option label="小于" value="7"></el-option>
        <el-option label="小于等于" value="8"></el-option>
        <!-- <el-option label="介于" value="9"></el-option> -->
    </el-select>
  </el-form-item>
    <el-form-item>
   <el-input v-model="whereVal" style="width: 200px;" class="filter-item" clearable></el-input>
  </el-form-item>
  <el-form-item>
      <el-button class="filter-item" type="primary" v-waves icon="search" v-on:click="Submit()">查询</el-button>
  </el-form-item>

</el-form>
</div>
</template>
<script>
import waves from '@/directive/waves/index.js' // 水波纹指令
export default {
  directives: {
    waves
  },
  props: {
    whereColArr: { type: Array }
  },
  data: function() {
    return {
      whereColValue: '',
      whereCon: '0',
      whereVal: '',
      showHtml: '<el-input v-model="whereVal" style="width: 200px;" class="filter-item"></el-input>'
    }
  },
  methods: {
    Submit() {
      const obj = {
        WhereCol: this.whereColValue,
        WhereCon: this.whereCon,
        WhereVal: this.whereVal
      }
      this.$emit('Submit', obj)
    },
    SelectOption(item) {
      if (item.type === 'date') {
        this.showHtml = ''
      } else if (item.type === 'select') {
        this.showHtml = ''
      } else if (item.type === 'undefined') {
        this.showHtml = '<el-input v-model="whereVal" style="width: 200px;" class="filter-item"></el-input>'
      }
    }
  }
}
</script>
