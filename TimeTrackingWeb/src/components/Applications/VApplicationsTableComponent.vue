<template xmlns:v-slot="http://www.w3.org/1999/XSL/Transform">
  <v-card>
    <v-card-title primary-title>
      <h3 class="mb-0">Приложения</h3>
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
      <template v-if="desserts.length">
        <v-btn color="primary" dark class="mb-2" @click="ImportXLSXList">Экспорт в xlsx</v-btn>
        <v-btn color="primary" dark class="mb-2" @click="ImportCSVList">Экспорт в csv</v-btn>
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
        <td class="justify-center layout px-0 align-center">
          <v-icon
            small
            class="mr-2"
            @click="$emit('on-edit', props.item.id)"
          >
            edit
          </v-icon>
        </td>
        <td>{{ GetUpdatedAt(props.item.updatedAt) }}</td>
        <td>{{ props.item.caption }}</td>
        <td>{{ GetCurrentState(props.item.state) }}</td>
      </template>
      <v-alert v-slot:no-results :value="true" color="error" icon="warning">
        По запросу "{{search}}" ничего не найдено.
      </v-alert>
    </v-data-table>
  </v-card>
</template>

<script lang="ts">
import { Component, Mixins, Watch } from 'vue-property-decorator'
import {
  Applications, SortingRequest, TableSortingRequest
} from '%/stores/api/SwaggerDocumentationTypescript'
import SkipTake from '%/utils/SkipTake'
import { DownloadingFileForBrowsers, FileFormatEnum } from '%/constants/DownloadingFileForBrowsers'

const filename = 'application'

@Component
export default class VApplicationsTableComponent extends Mixins(SkipTake) {
  desserts: Applications[] = []
  rowsPerPageItems: number[] = [5, 10, 25, 50, 100]
  headers = [
    { sortable: false, text: 'Действия' },
    { text: 'Обновлено', value: 'UpdatedAt' },
    { text: 'Название приложения', value: 'Caption' },
    { text: 'Состояние', value: 'State' }
  ]

  get dataRequest () {
    return new TableSortingRequest({
      sorting: new SortingRequest({
        descending: this.pagination.descending,
        sortBy: this.pagination.sortBy
      }),
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

  ImportXLSXList () {
    this.$store.state.api.applications_ImportXLSXGetListWithoutFilter(this.dataRequest)
      .then(res => DownloadingFileForBrowsers(res, filename, FileFormatEnum.XLSX))
      .catch(res => this.$root.$emit('snackbar', res))
  }

  ImportCSVList () {
    this.$store.state.api.applications_ImportCSVGetListWithoutFilter(this.dataRequest)
      .then(res => DownloadingFileForBrowsers(res, filename, FileFormatEnum.CSV))
      .catch(res => this.$root.$emit('snackbar', res))
  }

  loadApplicationList () {
    this.loading = true
    this.$store.state.api.applications_GetListWithoutFilter(this.dataRequest)
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
