<template>
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
            prepend-icon="user"
            name="login"
            label="Email"
            type="text"
            required
          />
          <v-text-field
            v-validate="'required|min:4'"
            :error-messages="errors.collect('password')"
            v-model="password"
            prepend-icon="unlock-alt"
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
</template>

<script lang="ts">
import { Component, Inject, Vue } from 'vue-property-decorator'
import { Validator } from 'vee-validate'
import { AuthenticateRequest } from '%/stores/api/SwaggerDocumentationTypescript'

@Component
export default class VAuthComponent extends Vue {
  @Inject('$validator') public $validator!: Validator

  login: string = ''
  password: string = ''

  mounted () {
    const vm = this
    this.$nextTick(function () {
      (vm.$refs.login as HTMLFormElement).focus()
    })
  }

  auth () {
    const authenticateRequest = new AuthenticateRequest({
      email: this.login,
      password: this.password
    })
    this.$store.state.api.users_Authenticate(authenticateRequest)
      .then(res => {
        this.$store.dispatch('setToken', res.token).then()
      })
  }

  submit () {
    this.$validator.validateAll()
      .then((res) => {
        if (res) {
          this.auth()
        }
      })
  }
}
</script>
