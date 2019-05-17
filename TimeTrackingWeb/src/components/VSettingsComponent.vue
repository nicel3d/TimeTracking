<template xmlns:v-slot="http://www.w3.org/1999/XSL/Transform">
  <div>
    <v-card>
      <v-card-title primary-title>
        <h3 class="mb-0">Настройки по сбору данных</h3>
        <v-spacer></v-spacer>
      </v-card-title>

      <v-card-text>
        <v-layout
          v-for="(day, key) in daysName"
          :key="key"
          row wrap>
          <v-flex xs12>
            {{ day }}
          </v-flex>
          <v-flex xs12 md10>
            <v-layout row>
              <v-flex xs6>
                <v-time-dialog-text-field
                  label="Рабоче время с"
                  v-model="settings[key].timeWorkingFrom"
                />
              </v-flex>
              <v-flex xs6>
                <v-time-dialog-text-field
                  label="Рабоче время по"
                  v-model="settings[key].timeWorkingTo"
                />
              </v-flex>
            </v-layout>
            <v-layout row>
              <v-flex xs6>
                <v-time-dialog-text-field
                  label="Время отдыха с"
                  v-model="settings[key].timeBreakFrom"
                />
              </v-flex>
              <v-flex xs6>
                <v-time-dialog-text-field
                  label="Время отдыха по"
                  v-model="settings[key].timeBreakTo"
                />
              </v-flex>
            </v-layout>
            <v-layout row>
              <v-flex>
                <v-text-field
                  v-model="settings[key].timeTheadMiliseconds"
                  label="Время задержки в миллисекундах"
                  type="number"
                  prepend-icon="access_time"
                  required
                />
              </v-flex>
            </v-layout>
          </v-flex>
          <v-flex>
            <v-checkbox
              class="ml-3"
              v-model="settings[key].status"
              :label="settings[key].status ? 'Включен' : 'Отключен'"
              hide-details
            />
          </v-flex>
        </v-layout>
      </v-card-text>
      <v-divider></v-divider>
      <v-card-actions>
        <v-spacer/>
        <v-btn
          color="primary"
          @click.prevent="onSave">Обновить
        </v-btn>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { Settings } from '%/stores/api/SwaggerDocumentationTypescript'
import VTimeDialogTextField from '%/utils/VTimeDialogTextField.vue'

@Component({
  components: { VTimeDialogTextField }
})
export default class VSettingsComponent extends Vue {
  loading: boolean = false
  settings: Settings[] = []
  daysName: string[] = [
    'Понедельник',
    'Вторник',
    'Среда',
    'Четверг',
    'Пятница',
    'Суббота',
    'Воскресенье'
  ]

  created () {
    this.settings = Array.from(new Array<number>(this.daysName.length))
      .map((x, index) => new Settings({
        id: index + 1,
        status: true,
        timeBreakFrom: '12:00',
        timeBreakTo: '13:00',
        timeWorkingFrom: '08:00',
        timeWorkingTo: '17:00',
        timeTheadMiliseconds: 500
      }))
  }

  mounted () {
    this.loadSettings()
  }

  loadSettings () {
    this.loading = true
    this.$store.state.api.settings_GetList()
      .then(res => {
        if (res.length) {
          res.forEach(item => {
            this.$set(this.settings, this.settings.findIndex(x => x.id === item.id), item)
          })
        }
      })
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }

  onSave () {
    this.loading = true
    this.$store.state.api.settings_Post(this.settings)
      .then(() => this.$root.$emit('snackbar-success', 'Успешно сохранено'))
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }
}
</script>
