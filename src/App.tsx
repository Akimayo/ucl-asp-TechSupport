import React from 'react';
import './App.scss';
import { Layout, Header, HeaderRow, Navigation, Drawer, Content } from 'react-mdl';
import Report from './Report';

interface INavigationLink {
  href: string;
  text: string;
}

export default class App extends React.Component {
  componentWillMount() {
    document.title = 'Report an Issue - ' + this.title;
  }
  render() {
    let counter = 0;
    return (
      <Layout className='layout-transparent mdl-layout--no-desktop-drawer-button'>
        <Header transparent={true}>
          <HeaderRow title={this.title}>
            <Navigation>
              {this.links.map(link => <a href={link.href} key={counter++}>{link.text}</a>)}
            </Navigation>
          </HeaderRow>
        </Header>
        <Drawer title={this.title}>
          <Navigation>
            {this.links.map(link => <a href={link.href} key={counter++}>{link.text}</a>)}
          </Navigation>
        </Drawer>
        <Content>
          <Report />
        </Content>
      </Layout>
    );
  }

  private readonly title: string = 'TechSupport';

  private links: ReadonlyArray<INavigationLink> = [
    {
      href: 'https://localhost:5001',
      text: 'Resolve Reports'
    } as INavigationLink
  ];
}