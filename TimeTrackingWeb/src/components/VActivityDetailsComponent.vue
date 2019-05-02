<template xmlns:v-slot="http://www.w3.org/1999/XSL/Transform">
  <v-layout row justify-center>
    <v-dialog v-model="dialog" fullscreen hide-overlay transition="dialog-bottom-transition">
      <v-card>
        <v-progress-linear v-if="loading" color="blue" indeterminate></v-progress-linear>
        <template v-else>
          <v-toolbar dark color="primary">
            <v-btn icon dark @click="dialog = false">
              <v-icon>close</v-icon>
            </v-btn>
            <v-toolbar-title>Детали активности</v-toolbar-title>
            <v-spacer></v-spacer>
            <v-toolbar-items>
              <v-btn dark flat @click="onEdit">Сохранить</v-btn>
            </v-toolbar-items>
          </v-toolbar>

          <v-container grid-list-xl>
            <v-layout wrap justify-space-between>
              <v-flex xs12>
                <div class="mb-4">
                  <vue-picture-swipe class="text-xs-center image-origin" :items="[image]"></vue-picture-swipe>
                </div>
              </v-flex>

              <v-flex xs12 md6>
                <v-text-field
                  :value="item.application.caption"
                  label="Название программы"
                  readonly
                ></v-text-field>
              </v-flex>
              <v-flex xs12 md6>
                <v-text-field
                  :value="item.applicationTitle"
                  label="Заголовок программы"
                  readonly
                ></v-text-field>
              </v-flex>

              <v-flex xs12 md6>
                <v-text-field
                  :value="item.staff.caption"
                  label="Пользователь"
                  readonly
                ></v-text-field>
              </v-flex>

              <v-flex xs12 md6>
                <v-text-field
                  :value="item.updatedAt.toLocaleDateString()"
                  label="Дата"
                  readonly
                ></v-text-field>
              </v-flex>

              <v-flex xs12 md6>
                <v-text-field
                  :value="item.updatedAt.toLocaleDateString('ru', {weekday: 'long'})"
                  label="День недели"
                  readonly
                ></v-text-field>
              </v-flex>

              <v-flex xs12 sm6>
                <v-select
                  v-model="state"
                  :items="states"
                  item-value="state"
                  item-text="text"
                  single-line
                  persistent-hint
                  label="Статус"
                ></v-select>
              </v-flex>
            </v-layout>
          </v-container>
          <v-divider></v-divider>
        </template>
      </v-card>
    </v-dialog>
  </v-layout>
</template>

<script lang="ts">
import { Component } from 'vue-property-decorator'
import SkipTake from '%/utils/SkipTake'
import { ActivityStaff, StateEnum } from '%/stores/api/SwaggerDocumentationTypescript'
import { oc } from 'ts-optchain'
import { States } from '%/constants/States'

@Component
export default class VActivityDetailsComponent extends SkipTake {
  item!: ActivityStaff
  dialog: boolean = false
  loading: boolean = true
  state: StateEnum = StateEnum.Neutral
  states = States

  get image () {
    return {
      src: 'data:image/jpeg;base64,' + oc(this.item).imageOrigin(''),
      thumbnail: 'data:image/jpeg;base64,' + oc(this.item).imageOrigin(''),
      w: 1920,
      h: 1080,
      title: oc(this.item).application.caption('')
    }
  }

  onView (id) {
    this.dialog = true
    this.loading = true
    this.$store.state.api.activityStaff_Get(id)
      .then(res => (this.item = res))
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }

  onEdit () {
    if (!this.item.applicationId || !this.item.application) return

    this.loading = true
    this.item.application.state = this.state
    this.$store.state.api.activityStaff_Put(this.item.applicationId, this.item.application)
      .then(() => (this.dialog = false))
      .catch(res => this.$root.$emit('snackbar', res))
      .then(() => (this.loading = false))
  }
}
</script>

<style>
.image-origin [itemprop="thumbnail"] {
  max-width: 100%;
  max-height: 450px;
}
</style>
