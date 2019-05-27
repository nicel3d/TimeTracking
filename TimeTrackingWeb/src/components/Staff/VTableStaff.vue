<template xmlns:v-slot="http://www.w3.org/1999/XSL/Transform">
  <v-card>
    <v-card-title primary-title>
      <h3 class="mb-0">Пользователи</h3>
      <v-spacer></v-spacer>
      <v-text-field
        v-model="search"
        @change="onPagination"
        append-icon="search"
        label="Поиск"
        single-line
        hide-details
      ></v-text-field>
    </v-card-title>
    <div class="mx-2">
      <v-btn color="primary" v-if="groupId" dark class="mb-2" @click="onAdd">Добавить в группу</v-btn>
      <template v-if="desserts.length">
        <v-btn color="primary" dark class="mb-2" @click="ExportXLSXList">Экспорт в xlsx</v-btn>
        <v-btn color="primary" dark class="mb-2" @click="ExportCSVList">Экспорт в csv</v-btn>
      </template>
    </div>
    <v-data-table
      :headers="headers"
      :items="desserts"
      :loading="loading"
      :rows-per-page-items="rowsPerPageItems"
      :pagination.sync="pagination"
      :total-items="totalDesserts"
    >
      <v-progress-linear v-slot:progress color="blue" indeterminate></v-progress-linear>
      <template v-slot:items="props">
        <td class="ml-4 mt-0 layout px-0 align-center">
          <v-icon
            small
            @click="groupId ? onDelete(props.item.id, groupId) : onEdit(props.item.id)">
            {{ groupId ? 'delete' : 'edit' }}
          </v-icon>
        </td>
        <td>{{ GetUpdatedAt(props.item.updatedAt) }}</td>
        <td>{{ props.item.caption }}</td>
        <td>{{ GetUpdatedAt(props.item.activityFirst) }}</td>
        <td>{{ props.item.rangeLastActivityTime }}</td>
        <td>{{ GetUpdatedAt(props.item.activityLast) }}</td>
      </template>
      <v-alert v-slot:no-results :value="true" color="error" icon="warning">
        По запросу "{{search}}" ничего не найдено.
      </v-alert>
    </v-data-table>
  </v-card>
</template>

<script lang="ts">
import { Component, Mixins, Prop, Watch } from 'vue-property-decorator'
import {
  SortingRequest, Staff, StaffToGroup,
  TableSortingByGroupIdRequest
} from '%/stores/api/SwaggerDocumentationTypescript'
import SkipTake from '%/utils/SkipTake'
import { DownloadingFileForBrowsers, FileFormatEnum } from '%/constants/DownloadingFileForBrowsers'
import { StaffEmitEnum, StaffIdsAndGroupId } from '%/constants/WindowsEmmit'

const filename = 'staff'

@Component
export default class VTableStaff extends Mixins(SkipTake) {
  @Prop({ default: null }) groupId

  desserts: Staff[] = []
  rowsPerPageItems: number[] = [5, 10, 25, 50, 100]
  headers = [
    { sortable: false, text: 'Действия' },
    { text: 'Обновлено', value: 'UpdatedAt' },
    { text: 'Пользователь', value: 'Caption' },
    { text: 'Последнее подключение', value: 'ActivityFirst' },
    { text: 'Продолжительность последнего сеанса', value: 'RangeLastActivityTime' },
    { text: 'Последнее отключение', value: 'ActivityLast' }
  ]

  get dataRequest () {
    return new TableSortingByGroupIdRequest({
      sorting: new SortingRequest({
        descending: this.pagination.descending,
        sortBy: this.pagination.sortBy
      }),
      groupId: this.groupId || undefined,
      search: this.search,
      skip: this.skip,
      take: this.take
    })
  }

  @Watch('pagination')
  onPagination () {
    if (!this.loading) {
      this.loadStaffList()
    }
  }

  mounted () {
    this.loadStaffList()
    this.$root.$on(StaffEmitEnum.CHANGE_STAFF_SUCCESS, this.loadStaffList)
  }

  onAdd () {
    this.$root.$emit(
      StaffEmitEnum.ADD_STAFF_TO_GROUP,
      new StaffIdsAndGroupId({
        groupId: this.groupId,
        staffIds: this.desserts.length ? this.desserts.map(x => Number(x.id)) : []
      })
    )
  }

  onEdit = id => this.$root.$emit(StaffEmitEnum.EDIT_STAFF, id)

  onDelete (staffId: number, groupId: number) {
    const data = new StaffToGroup({ staffId, groupId })
    this.$store.state.api.group_DeleteStaffToGroup(data)
      .then(this.onPagination)
      .catch(res => this.$root.$emit('snackbar', res))
  }

  ExportXLSXList () {
    this.$store.state.api.staff_ExportXLSXGetListWithoutFilter(this.dataRequest)
      .then(res => DownloadingFileForBrowsers(res, filename, FileFormatEnum.XLSX))
      .catch(res => this.$root.$emit('snackbar', res))
  }

  ExportCSVList () {
    this.$store.state.api.staff_ExportCSVGetListWithoutFilter(this.dataRequest)
      .then(res => DownloadingFileForBrowsers(res, filename, FileFormatEnum.CSV))
      .catch(res => this.$root.$emit('snackbar', res))
  }

  loadStaffList () {
    this.loading = true
    this.$store.state.api.staff_GetList(this.dataRequest)
      .then(res => {
        this.desserts = res.data
        this.totalDesserts = res.total
      })
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }

  beforeDestroy () {
    this.$root.$off(StaffEmitEnum.CHANGE_STAFF_SUCCESS, this.loadStaffList)
  }
}
</script>

<style scoped>

</style>
