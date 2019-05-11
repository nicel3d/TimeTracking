<template xmlns:v-slot="http://www.w3.org/1999/XSL/Transform">
  <v-card>
    <v-card-title primary-title>
      <h3 class="mb-0">Группы</h3>
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
      <v-btn color="primary" dark class="mb-2" @click="onAdd">Добавить группу</v-btn>
      <v-btn color="primary" dark class="mb-2" @click="ImportXLSXApplicationList">Экспорт в xlsx</v-btn>
      <v-btn color="primary" dark class="mb-2" @click="ImportCSVApplicationList">Экспорт в csv</v-btn>
    </div>
    <v-data-table
      class="linked"
      :headers="headers"
      :items="desserts"
      :loading="loading"
      :rows-per-page-items="rowsPerPageItems"
      :pagination.sync="pagination"
      :total-items="totalDesserts">
      <v-progress-linear v-slot:progress color="blue" indeterminate></v-progress-linear>
      <template v-slot:items="props">
        <tr @click.prevent="loadUsersByGroupId(props)" :class="['link', {'active': !!props.expanded}]">
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
          <td>{{ props.item.name }}</td>
          <td>{{ props.item.countUsers }}</td>
        </tr>
      </template>
      <template v-slot:expand="props">
        <v-card flat>
          <v-card-title>Пользователи</v-card-title>
          <v-data-table
            style="max-width: 600px"
            :hide-actions="true"
            :headers="childrenHeaders"
            :items="props.item.childrenDesserts">
            <template v-slot:items="props">
              <td>
                {{ props.item.updatedAt.toLocaleDateString() }}
                {{ props.item.updatedAt.toLocaleTimeString('ru', {hour: '2-digit', minute:'2-digit'}) }}
              </td>
              <td>{{ props.item.caption }}</td>
            </template>
          </v-data-table>
        </v-card>
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
  GroupsWithCountUsers,
  SortingRequest,
  StateEnum, TableSortingByGroupIdRequest,
  TableSortingRequest
} from '%/stores/api/SwaggerDocumentationTypescript'
import { oc } from 'ts-optchain'
import SkipTake from '%/utils/SkipTake'
import { States } from '%/constants/States'
import { DownloadingFileForBrowsers, FileFormatEnum } from '%/constants/DownloadingFileForBrowsers'
import { GroupEmitEnum } from '%/constants/windows/GroupsWindows'

const filename = 'groups'

@Component
export default class VStaffTableComponent extends Mixins(SkipTake) {
  desserts: GroupsWithCountUsers[] = []
  rowsPerPageItems: number[] = [5, 10, 25, 50, 100]
  headers = [
    { sortable: false, text: 'Действия' },
    { text: 'Обновлено', value: 'UpdatedAt' },
    { text: 'Наименование группы', value: 'Name' },
    { text: 'Кол-во участников', value: 'ActivityFirst' }
  ]
  childrenHeaders = [
    { text: 'Обновлено', value: 'updatedAt' },
    { text: 'Пользователь', value: 'caption' }
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
      this.loadGroupList()
    }
  }

  mounted () {
    this.loadGroupList()
    this.$root.$on(GroupEmitEnum.ADD_GROUP_SUCCESS, this.loadGroupList)
  }

  onAdd = () => this.$root.$emit(GroupEmitEnum.ADD_GROUP)

  getState (state?: StateEnum) {
    return oc(States.find(item => item.state === state)).text(States[1].text)
  }

  ImportXLSXApplicationList () {
    this.$store.state.api.group_ImportXLSXGetListWithoutFilter(this.dataRequest)
      .then(res => DownloadingFileForBrowsers(res, filename, FileFormatEnum.XLSX))
      .catch(res => this.$root.$emit('snackbar', res))
  }

  ImportCSVApplicationList () {
    this.$store.state.api.group_ImportCSVGetListWithoutFilter(this.dataRequest)
      .then(res => DownloadingFileForBrowsers(res, filename, FileFormatEnum.CSV))
      .catch(res => this.$root.$emit('snackbar', res))
  }

  loadUsersByGroupId (props: any) {
    if (props.expanded) {
      props.expanded = false
      return
    }
    props.expanded = true
    this.loading = true
    const data = new TableSortingByGroupIdRequest({ groupId: props.item.id })
    this.$store.state.api.staff_GetListByUserId(data)
      .then(res => (props.item.childrenDesserts = res))
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }

  loadGroupList () {
    this.loading = true
    this.$store.state.api.group_GetListWithCountUsers(this.dataRequest)
      .then(res => {
        this.desserts = res.data
        this.totalDesserts = res.total
      })
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }

  beforeDestroy () {
    this.$root.$off(GroupEmitEnum.ADD_GROUP_SUCCESS, this.loadGroupList)
  }
}
</script>

<style lang="scss">
@import "../../assets/table";
</style>
