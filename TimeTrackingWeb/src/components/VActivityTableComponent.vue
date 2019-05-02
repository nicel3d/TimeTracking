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
        <td></td>
        <td>{{ props.item.applicationTitle }}</td>
        <td>
          <div class="mx-1 my-1">
            <img height="50" :src="'data:image/jpeg;base64,' + props.item.imageUrlSmall" alt="">
          </div>
        </td>
        <td>{{ props.item.staffAlias }}</td>
        <td></td>
      </template>
      <v-alert v-slot:no-results :value="true" color="error" icon="warning">
        Your search for "{{ search }}" found no results.
      </v-alert>
    </v-data-table>
  </v-card>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { ActivityStaffFull } from '%/stores/api/SwaggerDocumentationTypescript'

const nameof = <T> (name: Extract<keyof T, string>): string => name

@Component
export default class VActivityTableComponent extends Vue {
  desserts: ActivityStaffFull[] = []
  headers = [
    {
      sortable: false,
      text: 'Действия'
    },
    { text: 'Обновлено', value: nameof<ActivityStaffFull>('updatedAt') },
    { text: 'Название приложения', value: (item) => item },
    { text: 'Заголовок активнго окна', value: nameof<ActivityStaffFull>('applicationTitle') },
    {
      sortable: false,
      text: 'Слепок экрана',
      value: nameof<ActivityStaffFull>('imageUrlSmall')
    },
    { text: 'Пользователь', value: nameof<ActivityStaffFull>('staffAlias') },
    { text: 'Состояние', value: '' }
  ]

  editItem (item) {
  }

  mounted () {
    this.$store.state.api.activityStaff_GetAll()
      .then(res => {
        this.desserts = res
      })
  }
}
</script>

<style scoped>

</style>
