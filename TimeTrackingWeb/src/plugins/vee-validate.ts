import Vue from 'vue'
import VueI18n from 'vue-i18n'
import VeeValidate, { Validator } from 'vee-validate'
// @ts-ignore
import validationMessages from 'vee-validate/dist/locale/ru'

Vue.use(VueI18n)
const i18n = new VueI18n()

Vue.use(VeeValidate, {
  inject: false,
  i18nRootKey: 'validations',
  dictionary: {
    ru: validationMessages
  }
})
Validator.localize('ru')
