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
      <v-btn color="primary" dark class="mb-2" @click="ImportXLSXApplicationList">Экспорт в xlsx</v-btn>
      <v-btn color="primary" dark class="mb-2" @click="ImportCSVApplicationList">Экспорт в csv</v-btn>
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
        <td class="justify-center layout px-0 align-center">
          <v-icon
            small
            class="mr-2"
            @click="$emit('on-edit', props.item.id)"
          >
            edit
          </v-icon>
        </td>
        <td>
          {{ props.item.updatedAt.toLocaleDateString() }}
          {{ props.item.updatedAt.toLocaleTimeString('ru', {hour: '2-digit', minute:'2-digit'}) }}
        </td>
        <td>{{ props.item.caption }}</td>
        <td>-</td>
        <td>-</td>
        <td>-</td>
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
  SortingRequest, Staff,
  TableSortingByGroupIdRequest,
  TableSortingRequest
} from '%/stores/api/SwaggerDocumentationTypescript'
import SkipTake from '%/utils/SkipTake'
import { States } from '%/constants/States'
import { DownloadingFileForBrowsers, FileFormatEnum } from '%/constants/DownloadingFileForBrowsers'

const filename = 'staff'

@Component
export default class VStaffTableComponent extends Mixins(SkipTake) {
  @Prop({ default: null }) groupId

  desserts: Staff[] = []
  rowsPerPageItems: number[] = [5, 10, 25, 50, 100]
  headers = [
    { sortable: false, text: 'Действия' },
    { text: 'Обновлено', value: 'UpdatedAt' },
    { text: 'Пользователь', value: 'Caption' },
    { text: 'Последнее подключение', value: 'ActivityFirst' },
    { text: 'Продолжительность последнего сеанса', value: 'SessionDuration' },
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
      this.loadApplicationList()
    }
  }

  ImportXLSXApplicationList () {
    this.$store.state.api.staff_ImportXLSXGetListWithoutFilter(this.dataRequest)
      .then(res => DownloadingFileForBrowsers(res, filename, FileFormatEnum.XLSX))
      .catch(res => this.$root.$emit('snackbar', res))
  }

  ImportCSVApplicationList () {
    this.$store.state.api.staff_ImportCSVGetListWithoutFilter(this.dataRequest)
      .then(res => DownloadingFileForBrowsers(res, filename, FileFormatEnum.CSV))
      .catch(res => this.$root.$emit('snackbar', res))
  }

  loadApplicationList () {
    this.loading = true
    this.$store.state.api.staff_GetList(this.dataRequest)
      .then(res => {
        this.desserts = res.data
        this.totalDesserts = res.total
      })
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }

  mounted () {
    this.loadApplicationList()
  }
}
</script>

<style scoped>

</style>
