<template xmlns:v-slot="http://www.w3.org/1999/XSL/Transform">
  <v-card>
    <v-card-title primary-title>
      <h3 class="mb-0">Заголовки приложения</h3>
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
        <v-btn color="primary" dark class="mb-2" @click.prevent>Экспорт в xlsx</v-btn>
        <v-btn color="primary" dark class="mb-2" @click.prevent>Экспорт в csv</v-btn>
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
        <td>{{ props.item.applicationCaption }}</td>
        <td>{{ GetCurrentMode(props.item.state) }}</td>
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
  ApplicationTitles, ApplicationTitlesFilterRequest, ApplicationTitleToGroup, GroupApplicationTitlesVM, SortingRequest
} from '%/stores/api/SwaggerDocumentationTypescript'
import SkipTake from '%/utils/SkipTake'
import {
  ApplicationsIdsAndGroupId,
  ApplicationTitleEmitEnum,
  ApplicationTitlesIdsAndApplicationId, GroupApplicationTitleEmitEnum
} from '%/constants/WindowsEmmit'

@Component
export default class VTableGroupApplicationTitles extends Mixins(SkipTake) {
  @Prop({ default: null }) groupId!: number

  desserts: ApplicationTitles[] = []
  rowsPerPageItems: number[] = [5, 10, 25, 50, 100]
  headers = [
    { sortable: false, text: 'Действия' },
    { text: 'Обновлено', value: 'UpdatedAt' },
    { text: 'Название приложения', value: 'ApplicationCaption' },
    { text: 'Мод', value: 'Mode' },
    { text: 'Текст', value: 'ApplicationTitle' },
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
      this.loadList()
    }
  }

  mounted () {
    this.$root.$on(GroupApplicationTitleEmitEnum.CHANGE_GROUP_APPLICATION_TITLE, this.loadList)
    this.loadList()
  }

  onAdd () {
    this.$root.$emit(
      GroupApplicationTitleEmitEnum.ADD_GROUP_APPLICATION_TITLE,
      new ApplicationsIdsAndGroupId({ groupId: this.groupId })
    )
  }

  onEdit (item: GroupApplicationTitlesVM) {
    this.$root.$emit(
      GroupApplicationTitleEmitEnum.EDIT_GROUP_APPLICATION_TITLE,
      new ApplicationTitleToGroup({
        ...item,
        title: item.applicationTitle
      })
    )
  }

  onDelete (id: number) {
    this.$store.state.api.applications_Delete(id)
      .then(this.onPagination)
      .catch(res => this.$root.$emit('snackbar', res))
  }

  loadList () {
    this.loading = true
    this.$store.state.api.groupApplicationTitles_GetList(this.dataRequest)
      .then(res => {
        this.desserts = res.data
        this.totalDesserts = res.total
      })
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }

  beforeDestroy () {
    this.$root.$off(GroupApplicationTitleEmitEnum.CHANGE_GROUP_APPLICATION_TITLE, this.loadList)
  }
}
</script>

<style scoped>

</style>
