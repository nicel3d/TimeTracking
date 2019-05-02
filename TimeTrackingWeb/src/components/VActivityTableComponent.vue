<template xmlns:v-slot="http://www.w3.org/1999/XSL/Transform">
  <v-card>
    <v-card-title>
      Nutrition
      <v-spacer></v-spacer>
      <v-text-field
        v-model="search"
        append-icon="search"
        label="Search"
        single-line
        hide-details
      ></v-text-field>
    </v-card-title>
    <v-data-table
      :headers="headers"
      :items="desserts"
      :search="search"
      :pagination.sync="pagination"
    >
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
    {{ pagination }}
  </v-card>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { ActivityStaff, SkipTakeRequest } from '%/stores/api/SwaggerDocumentationTypescript'
import { oc } from 'ts-optchain'

const nameof = <T> (name: Extract<keyof T, string>): string => name

@Component
export default class VActivityTableComponent extends Vue {
  desserts: ActivityStaff[] = []
  search: string = ''
  pagination: DataTablePagination = {}
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

  editItem (item) {
  }

  mounted () {
    const data = new SkipTakeRequest({
      skip: 0,
      take: 10
    })
    this.$store.state.api.activityStaff_GetList(data)
      .then(res => {
        this.desserts = res.data
      })
  }
}
</script>

<style scoped>

</style>
