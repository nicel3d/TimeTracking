<template>
  <v-dialog v-model="dialog" max-width="300px">
    <v-card>
      <v-card-title>Добавление пользователя в группу</v-card-title>
      <v-divider></v-divider>
      <v-card-text>
        <v-form :data-vv-scope="formId" @submit.prevent="onAdd">
          <v-autocomplete
            v-model="staffId"
            :items="staffList"
            label="Пользователь"
            data-vv-as="Имя пользователя"
            :data-vv-name="`${formId}.name`"
            item-text="value"
            item-value="name"
            persistent-hint
            :loading="loading"
            v-validate="'required'"
            :error-messages="errors.collect(`${formId}.name`)"
          />
        </v-form>
      </v-card-text>
      <v-divider></v-divider>
      <v-card-actions>
        <v-spacer/>
        <v-btn
          color="blue darken-1"
          flat
          @click.prevent="onReset">Отменить
        </v-btn>
        <v-btn
          color="primary"
          @click.prevent="onAdd">Добавить
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script lang="ts">
import { Component, Inject, Vue } from 'vue-property-decorator'
import { Validator } from 'vee-validate'
import { StaffToGroup, TableSortingByGroupIdRequest } from '%/stores/api/SwaggerDocumentationTypescript'
import { StaffEmitEnum, StaffIdsAndGroupId } from '%/constants/WindowsEmmit'

@Component
export default class VStaffAddGroupWindow extends Vue {
  @Inject('$validator') public $validator!: Validator

  dialog: boolean = false
  name: string = ''
  formId: string = 'form-add-group-to-staff'
  loading: boolean = false
  staffId: number | null = null
  staffList: object[] = []
  staffIds: number[] = []
  groupId!: number
  $refs: any
  $options: any

  onReset () {
    Object.assign(this.$data, this.$options.data.call(this))
  }

  mounted () {
    this.$root.$on(StaffEmitEnum.ADD_STAFF_TO_GROUP, this.onOpenWindow)
  }

  onOpenWindow (request: StaffIdsAndGroupId) {
    const { groupId, staffIds } = request
    this.dialog = true
    this.staffIds = staffIds
    this.groupId = groupId
    this.loadStaffList()
  }

  onAdd () {
    this.$validator.validateAll(this.formId)
      .then((res) => {
        if (res && this.staffId) {
          const data = new StaffToGroup({
            staffId: this.staffId,
            groupId: this.groupId
          })
          this.$store.state.api.group_PostStaffToGroup(data)
            .then(() => {
              this.$root.$emit(StaffEmitEnum.CHANGE_STAFF_SUCCESS)
              this.onReset()
            })
            .catch(res => this.$root.$emit('snackbar', res))
        }
      })
  }

  loadStaffList () {
    this.loading = true
    this.$store.state.api.staff_GetList(new TableSortingByGroupIdRequest())
      .then(res => {
        this.staffList = res.data
          .filter(x => !this.staffIds.includes(x.id))
          .map(x => ({
            name: x.id,
            value: x.caption
          }))
      })
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }

  beforeDestroy () {
    this.$root.$off(StaffEmitEnum.ADD_STAFF_TO_GROUP, this.onOpenWindow)
  }
}

</script>

<style scoped>

</style>
