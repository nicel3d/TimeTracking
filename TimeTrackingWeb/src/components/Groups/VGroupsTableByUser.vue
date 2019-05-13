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
      <!--      <template v-if="desserts.length">-->
      <!--        <v-btn color="primary" dark class="mb-2" @click="ImportXLSXList">Экспорт в xlsx</v-btn>-->
      <!--        <v-btn color="primary" dark class="mb-2" @click="ImportCSVList">Экспорт в csv</v-btn>-->
      <!--      </template>-->
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
        <tr>
          <td class="layout px-0 align-center">
            <v-icon
              small
              class="ml-4 mr-2"
              @click="onEdit(props.item.id)">
              edit
            </v-icon>
          </td>
          <td>{{ GetUpdatedAt(props.item.updatedAt) }}</td>
          <td>{{ props.item.name }}</td>
          <td class="align-center layout">
            <v-checkbox
              :input-value="groupIds.length && groupIds.map(x => x.groupId).includes(props.item.id)"
              @click.stop="onChangeStatus(props.item.id)"
              hide-details
              readonly
            />
          </td>
        </tr>
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
  GroupsWithCountUsers,
  SortingRequest, StaffToGroup, StateEnum,
  TableSortingRequest
} from '%/stores/api/SwaggerDocumentationTypescript'
import { oc } from 'ts-optchain'
import SkipTake from '%/utils/SkipTake'
import { States } from '%/constants/States'
import { DownloadingFileForBrowsers, FileFormatEnum } from '%/constants/DownloadingFileForBrowsers'
import { GroupEmitEnum } from '%/constants/WindowsEmmit'

const filename = 'groups'

@Component
export default class VGroupsTableByUser extends Mixins(SkipTake) {
  @Prop({ default: 0 }) staffId!: number
  @Prop({ default: [] }) groupIds!: StaffToGroup[]

  desserts: GroupsWithCountUsers[] = []
  rowsPerPageItems: number[] = [5, 10, 25, 50, 100]
  headers = [
    { sortable: false, text: 'Действия' },
    { text: 'Обновлено', value: 'UpdatedAt' },
    { text: 'Наименование группы', value: 'Name' },
    { sortable: false, text: 'Группа пользователя' }
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
    this.$root.$on(GroupEmitEnum.CHANGE_GROUP_SUCCESS, this.loadGroupList)
  }

  onAdd = () => this.$root.$emit(GroupEmitEnum.ADD_GROUP)
  onEdit = (id) => this.$root.$emit(GroupEmitEnum.EDIT_GROUP, id)

  onChangeStatus (groupId: number) {
    const staffToGroup = this.groupIds.length && this.groupIds
      .find(x => x.groupId === groupId && x.staffId === this.staffId)

    if (staffToGroup) {
      this.onDeleteGroupForUser(staffToGroup)
    } else {
      this.onSetGroupForUser(groupId)
    }
  }

  onSetGroupForUser (groupId: number) {
    this.loading = true
    const data = new StaffToGroup({
      groupId: groupId,
      staffId: this.staffId
    })
    this.$store.state.api.group_PostStaffToGroup(data)
      .then(res => (this.groupIds.push(res)))
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }

  onDeleteGroupForUser (staffToGroup: StaffToGroup) {
    this.loading = true
    const data = new StaffToGroup({
      groupId: staffToGroup.groupId,
      staffId: staffToGroup.staffId
    })
    this.$store.state.api.group_DeleteStaffToGroup(data)
      .then(() => {
        this.$emit('update:groupIds', this.groupIds
          .filter(x => !(x.staffId === data.staffId && x.groupId === data.groupId)))
      })
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }

  // ImportXLSXList () {
  //   this.$store.state.api.group_ImportXLSXGetListWithoutFilter(this.dataRequest)
  //     .then(res => DownloadingFileForBrowsers(res, filename, FileFormatEnum.XLSX))
  //     .catch(res => this.$root.$emit('snackbar', res))
  // }
  //
  // ImportCSVList () {
  //   this.$store.state.api.group_ImportCSVGetListWithoutFilter(this.dataRequest)
  //     .then(res => DownloadingFileForBrowsers(res, filename, FileFormatEnum.CSV))
  //     .catch(res => this.$root.$emit('snackbar', res))
  // }

  loadGroupList () {
    this.loading = true
    this.$store.state.api.group_GetList(this.dataRequest)
      .then(res => {
        this.desserts = res.data
        this.totalDesserts = res.total
      })
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }

  beforeDestroy () {
    this.$root.$off(GroupEmitEnum.CHANGE_GROUP_SUCCESS, this.loadGroupList)
  }
}
</script>

<style lang="scss">
@import "../../assets/table";
</style>
