<template xmlns:v-slot="http://www.w3.org/1999/XSL/Transform">
  <v-card>
    <v-card-title primary-title>
      <h3 class="mb-0">Ограничения по программам</h3>
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
      <v-btn color="primary" dark class="mb-2" @click="onAdd">Добавить обработчик</v-btn>
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
          <v-icon small @click="onEdit(props.item)">
            edit
          </v-icon>
          <v-icon class="ml-4" small @click="onDelete(props.item.id)">
            delete
          </v-icon>
        </td>
        <td>{{ GetUpdatedAt(props.item.updatedAt) }}</td>
        <td>{{ props.item.applicationTitle }}</td>
        <td>{{ GetCurrentState(props.item.state) }}</td>
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
  ApplicationGroupFilterRequest,
  SortingRequest, VMApplicationGroup
} from '%/stores/api/SwaggerDocumentationTypescript'
import SkipTake from '%/utils/SkipTake'
import { DownloadingFileForBrowsers, FileFormatEnum } from '%/constants/DownloadingFileForBrowsers'
import { ApplicationGroupEmitEnum, ApplicationsIdsAndGroupId } from '%/constants/WindowsEmmit'

const filename = 'program_restrictions'

@Component
export default class VTableApplicationGroups extends Mixins(SkipTake) {
  @Prop({ default: null }) groupId!: number

  desserts: VMApplicationGroup[] = []
  rowsPerPageItems: number[] = [5, 10, 25, 50, 100]
  headers = [
    { sortable: false, text: 'Действия' },
    { text: 'Обновлено', value: 'UpdatedAt' },
    { text: 'Программа', value: 'ApplicationTitle' },
    { text: 'Состояние', value: 'State' }
  ]

  get dataRequest () {
    return new ApplicationGroupFilterRequest({
      sorting: new SortingRequest({
        descending: this.pagination.descending,
        sortBy: this.pagination.sortBy
      }),
      groupId: this.groupId,
      search: this.search,
      skip: this.skip,
      take: this.take
    })
  }

  @Watch('pagination')
  onPagination () {
    if (!this.loading) {
      this.loadTreatmentApplications()
    }
  }

  mounted () {
    this.$root.$on(
      ApplicationGroupEmitEnum.CHANGE_APPLICATION_GROUP_SUCCESS,
      this.loadTreatmentApplications
    )
    this.loadTreatmentApplications()
  }

  onAdd () {
    this.$root.$emit(
      ApplicationGroupEmitEnum.ADD_APPLICATION_GROUP,
      new ApplicationsIdsAndGroupId({
        groupId: this.groupId,
        applicationsIds: this.desserts.length
          ? this.desserts.map(x => Number(x.applicationId)) : []
      })
    )
  }

  onEdit = (item: VMApplicationGroup) =>
    this.$root.$emit(ApplicationGroupEmitEnum.EDIT_APPLICATION_GROUP, item)

  onDelete (id: number) {
    this.$store.state.api.treatmentApplications_Delete(id)
      .then(this.onPagination)
      .catch(res => this.$root.$emit('snackbar', res))
  }

  ExportXLSXList () {
    this.$store.state.api.treatmentApplications_ExportXLSXGetListWithoutFilter(this.dataRequest)
      .then(res => DownloadingFileForBrowsers(res, filename, FileFormatEnum.XLSX))
      .catch(res => this.$root.$emit('snackbar', res))
  }

  ExportCSVList () {
    this.$store.state.api.treatmentApplications_ExportCSVGetListWithoutFilter(this.dataRequest)
      .then(res => DownloadingFileForBrowsers(res, filename, FileFormatEnum.CSV))
      .catch(res => this.$root.$emit('snackbar', res))
  }

  loadTreatmentApplications () {
    this.loading = true
    this.$store.state.api.treatmentApplications_GetList(this.dataRequest)
      .then(res => {
        this.desserts = res.data
        this.totalDesserts = res.total
      })
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }

  beforeDestroy () {
    this.$root.$off(
      ApplicationGroupEmitEnum.CHANGE_APPLICATION_GROUP_SUCCESS,
      this.loadTreatmentApplications
    )
  }
}
</script>

<style scoped>

</style>
