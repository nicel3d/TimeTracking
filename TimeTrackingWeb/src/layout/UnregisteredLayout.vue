<template>
  <v-app id="inspire">
    <v-content>
      <v-container
        fluid
        class="blue-grey lighten-5"
        fill-height>
        <v-layout
          align-center
          justify-center>
          <v-flex
            xs12
            sm8
            md4>
            <v-card
              class="elevation-12"
              @keyup.enter.prevent="submit">
              <v-toolbar
                dark
                color="primary">
                <v-toolbar-title>Форма авторизации</v-toolbar-title>
                <v-spacer/>
              </v-toolbar>
              <v-card-text>
                <v-form>
                  <v-text-field
                    v-validate="'required|email'"
                    ref="login"
                    v-model="login"
                    :error-messages="errors.collect('email')"
                    data-vv-name="email"
                    prepend-icon="person"
                    name="login"
                    label="Email"
                    type="text"
                    required
                  />
                  <v-text-field
                    v-validate="'required|min:6'"
                    :error-messages="errors.collect('password')"
                    v-model="password"
                    prepend-icon="lock"
                    name="password"
                    data-vv-as="пароль"
                    data-vv-name="password"
                    label="Пароль"
                    type="password"
                    required
                  />
                </v-form>
              </v-card-text>
              <v-card-actions>
                <v-spacer/>
                <v-btn
                  color="primary"
                  @click.prevent="submit">Авторизоваться
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-flex>
        </v-layout>
      </v-container>
    </v-content>
  </v-app>
</template>

<script lang="ts">
import { Component, Inject, Vue } from 'vue-property-decorator'
import { RouterNameEnum } from '%/constants/RouterConstant'
import { Validator } from 'vee-validate'

@Component
export default class UnregisteredLayout extends Vue {
  @Inject('$validator') public $validator!: Validator

  login: string = ''
  password: string = ''
  errorMessages: string = ''
  formHasErrors: boolean = false

  mounted () {
    const vm = this
    this.$nextTick(function () {
      (vm.$refs.login as HTMLFormElement).focus()
    })
  }

  submit () {
    this.$validator.validateAll()
      .then((res) => {
        if (res) {
          this.$router.push({ name: RouterNameEnum.Dashboard })
        }
      })
  }
}
</script>
