import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;
    constructor(props) {
        super(props);
        this.state = { searchText: '', linkToSearch: '', engine: 'google', searchResults: '', loading: false };
        this.handleChange = this.handleChange.bind(this);
        this.handleSelectChange = this.handleSelectChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.handleReset = this.handleReset.bind(this);
    }

    handleChange = (field) => (event) => {
        this.setState({ [field]: event.target.value });
    }

    handleSelectChange = (field) => (e) => {
        let value = Array.from(e.target.selectedOptions, option => option.value);
        this.setState({ [field]: value });
    }

    handleSubmit(event) {
        this.populateData();
        event.preventDefault();
    }

    handleReset(event) {
        this.setState({ searchText: '', linkToSearch: '', engine: 'google', searchResults: '', loading: false });
        event.preventDefault();
    }


    async populateData() {
        let controller = this;
        controller.setState({ loading: true });
        const queryString = `SearchEngine/GetResults?keyword=${this.state.searchText}&linkToSearch=${this.state.linkToSearch}&engines=${this.state.engine}`;
        await fetch(queryString).then((response) => {
            if (response.ok) {
                return response.text();
            }
            else {
                return 'Error occured while processing request';
            }
        }).then((data) => {
            controller.setState({ searchResults: data, loading: false });
        }).catch(function (err) {
            console.warn(err);
            controller.setState({ searchResults: 'Error occured while processing request', loading: false });
        });

    }

  render () {
      let contents = this.state.loading
          ? 'Loading...'
          : this.state.searchResults;
      return (
          <form onSubmit={this.handleSubmit} onReset={this.handleReset}>
              <table>
                  <tbody>
                      <tr>
                          <td>Keyword:</td>
                          <td><input type="text" value={this.state.searchText} onChange={this.handleChange('searchText')} /></td>
                      </tr>
                      <tr>
                          <td>Link To Match:</td>
                          <td><input type="text" value={this.state.linkToSearch} onChange={this.handleChange('linkToSearch')} /></td>
                      </tr>
                      <tr>
                          <td>Search Engine:</td>
                          <td><select multiple={true} value={this.state.engine} onChange={this.handleSelectChange('engine')}>
                              <option value="google">Google</option>
                              <option value="bing">Bing</option>
                              <option value="duckduckgo">DuckDuckGo</option>
                          </select></td>
                      </tr>
                      <tr>
                          <td></td>
                          <td>
                              <input type="submit" value="Submit" />
                              <input type="reset" value="Reset" />
                          </td>
                      </tr>
                  </tbody>

              </table>

              <hr />
                Results: {contents}
          </form>
      );
  }
}
