<template>
  <v-dialog-full-window
    @on-save="onReset"
    :item="item"
    :loading="loading"
    v-model="dialog">
    <span slot="title">
      Детали пользователя: {{ item ? item.caption : 'нету информации' }}
    </span>
    <v-form>
      <v-container v-if="item" fluid grid-list-md>
        <v-layout row wrap>
          <v-flex xs12 md6>
            <v-text-field
              type="text"
              :value="GetUpdatedAt(item.activityFirst)"
              label="Последнее подключение"
              readonly
            />
          </v-flex>

          <v-flex xs12 md6>
            <v-text-field
              type="text"
              :value="GetUpdatedAt(item.activityLast)"
              label="Последнее отключение"
              readonly
            />
          </v-flex>
        </v-layout>
        <v-layout row wrap>
          <v-flex xs12 md6>
            <v-text-field
              type="text"
              :value="item.rangeLastActivityTime"
              label="Продолжительность последнего сеанса"
              readonly
            />
          </v-flex>

          <v-flex xs12 md6>
            <v-text-field
              type="text"
              :value="GetUpdatedAt(item.updatedAt)"
              label="Обновлено"
              readonly
            />
          </v-flex>
        </v-layout>
      </v-container>
      <v-groups-table-by-user
        v-if="this.item"
        :staff-id="item.id"
        :group-ids.sync="groupIds"
      />
    </v-form>
  </v-dialog-full-window>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { Staff } from '%/stores/api/SwaggerDocumentationTypescript'
import { StaffEmitEnum } from '%/constants/WindowsEmmit'
import VDialogFullWindow from '%/utils/VDialogFullWindow.vue'
import VGroupsTableByUser from '%/components/Groups/VTableGroupsByUser.vue'

@Component({
  components: { VGroupsTableByUser, VDialogFullWindow }
})
export default class VWindowEditStaff extends Vue {
  item: Staff | null = null
  dialog: boolean = false
  $refs: any
  $options: any
  loading: boolean = true
  groupIds: number[] = []
  id!: number

  onReset () {
    Object.assign(this.$data, this.$options.data.call(this))
  }

  mounted () {
    this.$root.$on(StaffEmitEnum.EDIT_STAFF, this.onOpenWindow)
  }

  onOpenWindow (id: number) {
    this.id = id
    this.dialog = true
    this.loadStaff()
  }

  loadStaff () {
    this.loading = true
    this.$store.state.api.staff_Get(this.id)
      .then(res => {
        this.item = res
        this.groupIds = res.staffToGroup || []
      })
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }

  beforeDestroy () {
    this.$root.$off(StaffEmitEnum.EDIT_STAFF, this.onOpenWindow)
  }
}
</script>

<style scoped>

</style>
