import { FileResponse } from '%/stores/api/SwaggerDocumentationTypescript'

export enum FileFormatEnum {
    XLSX = 'xlsx',
    CSV = 'csv'
}

export function DownloadingFileForBrowsers (data: FileResponse, filename: string, fileFormat: FileFormatEnum) {
  let url
  if (!window.navigator.msSaveOrOpenBlob) {
    url = window.URL.createObjectURL(new Blob([data.data], { type: data.data.type }))
  } else {
    url = window.navigator.msSaveOrOpenBlob(new Blob([data.data]), `${filename}.${fileFormat}`)
  }
  const link = document.createElement('a')
  link.href = url
  link.download = `${filename}.${fileFormat}`
  link.click()
  link.remove()
}
