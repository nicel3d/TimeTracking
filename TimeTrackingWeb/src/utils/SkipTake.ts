import { Component, Vue } from 'vue-property-decorator'

class DataTablePagination {
  descending?: boolean
  page?: number
  rowsPerPage?: number
  sortBy?: string
  totalItems?: number
}

@Component
export default class SkipTake extends Vue {
  search: string = ''
  loading: boolean = true
  pagination: DataTablePagination = {
    page: 1,
    rowsPerPage: 5
  }
  totalDesserts: number = 0

  get skip () {
    return this.pagination.page && this.pagination.page > 1 && this.pagination.rowsPerPage
      ? this.pagination.rowsPerPage * (this.pagination.page - 1) : 0
  }

  get take () {
    return this.pagination.rowsPerPage
  }
}
