<template xmlns:v-slot="http://www.w3.org/1999/XSL/Transform">
  <v-card>
    <v-card-title primary-title>
      <h3 class="mb-0">Активность пользователей</h3>
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
        <td>{{ props.item.application.caption }}</td>
        <td>{{ props.item.applicationTitle }}</td>
        <td>
          <div class="mx-1 my-1">
            <img height="50" :src="'data:image/jpeg;base64,' + props.item.imageThumb" alt="">
          </div>
        </td>
        <td>{{ props.item.staff.caption }}</td>
        <td>{{ getState(props.item.application.state) }}</td>
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
  ActivityStaff, FilterRequest, SortingRequest, StateEnum, TableSortingWithFilterRequest
} from '%/stores/api/SwaggerDocumentationTypescript'
import { oc } from 'ts-optchain'
import SkipTake from '%/utils/SkipTake'
import { States } from '%/constants/States'

@Component
export default class VActivityTableComponent extends Mixins(SkipTake) {
  @Prop() filter!: FilterRequest

  desserts: ActivityStaff[] = []
  rowsPerPageItems: number[] = [5, 10, 25, 50, 100]
  headers = [
    { sortable: false, text: 'Действия' },
    { text: 'Обновлено', value: 'UpdatedAt' },
    { text: 'Название приложения', value: 'Application.Caption' },
    { text: 'Заголовок активнго окна', value: 'ApplicationTitle' },
    { sortable: false, text: 'Слепок экрана' },
    { text: 'Пользователь', value: 'Staff.Caption' },
    { text: 'Состояние', value: 'Application.State' }
  ]

  @Watch('pagination')
  @Watch('filter')
  onPagination () {
    if (!this.loading) {
      this.loadActivityStaffList()
    }
  }

  getState (state?: StateEnum) {
    return oc(States.find(item => item.state === state)).text(States[1].text)
  }

  loadActivityStaffList (skip = this.skip, take = this.take) {
    this.loading = true
    const data = new TableSortingWithFilterRequest({
      sorting: new SortingRequest({
        descending: this.pagination.descending,
        sortBy: this.pagination.sortBy
      }),
      filter: this.filter,
      search: this.search,
      skip,
      take
    })
    this.$store.state.api.activityStaff_GetList(data)
      .then(res => {
        this.desserts = res.data
        this.totalDesserts = res.total
      })
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }

  mounted () {
    this.loadActivityStaffList()
  }
}
</script>

<style scoped>

</style>
