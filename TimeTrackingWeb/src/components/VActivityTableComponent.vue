<template xmlns:v-slot="http://www.w3.org/1999/XSL/Transform">
  <v-card>
    <v-card-title>
      Активность пользователей
      <v-spacer></v-spacer>
      <v-text-field
        v-model="search"
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
      :pagination.sync="pagination"
      :total-items="totalDesserts"
    >
      <v-progress-linear v-slot:progress color="blue" indeterminate></v-progress-linear>
      <template v-slot:items="props">
        <td class="justify-center layout px-0">
          <v-icon
            small
            class="mr-2"
            @click="editItem(props.item)"
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
        <td></td>
      </template>
      <v-alert v-slot:no-results :value="true" color="error" icon="warning">
        По запросу "{{search}}" ничего не найдено.
      </v-alert>
    </v-data-table>
  </v-card>
</template>

<script lang="ts">
import { Component, Watch } from 'vue-property-decorator'
import { ActivityStaff, SortingAndSkipTakeRequest } from '%/stores/api/SwaggerDocumentationTypescript'
import { oc } from 'ts-optchain'
import SkipTake from '%/utils/SkipTake'

const nameof = <T> (name: Extract<keyof T, string>): string => name

@Component
export default class VActivityTableComponent extends SkipTake {
  desserts: ActivityStaff[] = []
  headers = [
    {
      sortable: false,
      text: 'Действия'
    },
    { text: 'Обновлено', value: nameof<ActivityStaff>('updatedAt') },
    { text: 'Название приложения', value: (item: ActivityStaff) => oc(item).application.caption('') },
    { text: 'Заголовок активнго окна', value: nameof<ActivityStaff>('applicationTitle') },
    {
      sortable: false,
      text: 'Слепок экрана'
    },
    { text: 'Пользователь', value: (item: ActivityStaff) => oc(item).staff.caption('') },
    { text: 'Состояние', value: '' }
  ]

  @Watch('pagination')
  onPagination () {
      this.loadActivityStaffList()
  }

  editItem (item) {
  }

  loadActivityStaffList (skip = this.skip, take = this.take) {
    this.loading = true
    const data = new SortingAndSkipTakeRequest({
      descending: this.pagination.descending,
      sortBy: this.pagination.sortBy,
      skip,
      take
    })
    this.$store.state.api.activityStaff_GetList(data)
      .then(res => {
        this.desserts = res.data
        this.totalDesserts = res.count
      })
      .catch()
      .then(() => (this.loading = false))
  }

  mounted () {
    this.loadActivityStaffList()
  }
}
</script>

<style scoped>

</style>
