<template xmlns:v-slot="http://www.w3.org/1999/XSL/Transform">
  <div>
    <v-card>
      <v-card-title primary-title>
        <h3 class="mb-0">Настройки по сбору данных</h3>
        <v-spacer></v-spacer>
      </v-card-title>
      <v-card-text>
        {{ settingsNew }}
        <v-layout
          v-for="(day, key) in daysName"
          :key="key"
          row wrap>
          <v-flex xs12>
            {{ day }}
          </v-flex>
          <v-flex xs6 md4>
            <v-text-field
              :value="settings[key] ? settings[key].timeBreakFrom : null"
              label="Picker in dialog"
              prepend-icon="access_time"
              @input="r($event, key)"
            />
          </v-flex>
          <!--                  <v-flex xs6 md4>-->
          <!--                    <v-text-field-->
          <!--                      v-model="settings[key].timeBreakTo"-->
          <!--                      label="Picker in dialog"-->
          <!--                      prepend-icon="access_time"-->
          <!--                      readonly-->
          <!--                      v-on="on"-->
          <!--                    />-->
          <!--                  </v-flex>-->
          <!--                  <v-flex xs2 md4>-->
          <!--                    <v-checkbox-->
          <!--                      class="ml-3"-->
          <!--                      :value="1"-->
          <!--                      :label="true ? 'Включен' : 'Отключен'"-->
          <!--                      hide-details-->
          <!--                    />-->
          <!--                  </v-flex>-->
        </v-layout>
      </v-card-text>
    </v-card>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { Settings } from '%/stores/api/SwaggerDocumentationTypescript'

@Component
export default class VSettingsComponent extends Vue {
  time: any = null
  modal2: boolean = false
  loading: boolean = false
  settings!: Settings[]
  daysName: string[] = [
    'Понедельник',
    'Вторник',
    'Среда',
    'Четверг',
    'Пятница',
    'Суббота',
    'Воскресенье'
  ]

  get settingsNew () {
    return this.settings
  }

  r (ev, key) {
    // console.log(ev.target.value)
    // this.settings[key].timeBreakFrom = '123'
    // this.$set(this.settings, key, { ...this.settings[key], 'timeBreakFrom': '10' })
    // console.log(this.settings)
    // this.$set(this.settings, 'timeBreakFrom', '10')
  }

  created () {
    this.settings = Array.from(new Array<number>(this.daysName.length - 1))
      .map((x, index) => new Settings({
        id: index,
        status: true
      }))
    // this.loadSettings()
  }

  loadSettings () {
    this.loading = true
    this.$store.state.api.settings_GetList()
      .then(res => (this.settings = res))
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }
}
</script>
