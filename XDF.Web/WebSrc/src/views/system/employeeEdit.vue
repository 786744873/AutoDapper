<template>
     <div class="createPost-container">
    <el-form class="form-container" ref="postForm">
    <el-form-item style="margin-bottom: 40px;" prop="title">
              <MDinput name="name"  required :maxlength="100">
                标题
              </MDinput>
              <span v-show="true" class='title-prompt'>app可能会显示不全</span>
            </el-form-item>
    </el-form>
     </div>
</template>
<script>
import MDinput from '@/components/MDinput'
export default {
     name: 'articleDetail',
     components: { MDinput },
     data(){
         return{

         }
     }
}
</script>
<style rel="stylesheet/scss" lang="scss" scoped>
  @import "src/styles/mixin.scss";
  .title-prompt{
    position: absolute;
    right: 0px;
    font-size: 12px;
    top:10px;
    color:#ff4949;
  }
  .createPost-container {
    position: relative;
    .createPost-main-container {
      padding: 40px 45px 20px 50px;
      .postInfo-container {
        position: relative;
        @include clearfix;
        margin-bottom: 10px;
        .postInfo-container-item {
          float: left;
        }
      }
      .editor-container {
        min-height: 500px;
        margin: 0 0 30px;
        .editor-upload-btn-container {
            text-align: right;
            margin-right: 10px;
            .editor-upload-btn {
                display: inline-block;
            }
        }
      }
    }
    .word-counter {
      width: 40px;
      position: absolute;
      right: -10px;
      top: 0px;
    }
  }
</style>


