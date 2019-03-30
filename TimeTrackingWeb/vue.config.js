const path = require('path')

module.exports = {
  transpileDependencies: [
    'vuex-module-decorators',
    /node_modules[/\\\\]vuetify[/\\\\]/
  ],
  configureWebpack: {
    resolve: {
      alias: {
        '@': path.join(__dirname, '/src/'),
        '%': path.join(__dirname, '/src/')
      }
    }
  }
}
