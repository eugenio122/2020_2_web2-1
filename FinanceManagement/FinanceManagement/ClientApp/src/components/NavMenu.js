import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink, TabContent, TabPane, Nav } from 'reactstrap';
import { Link } from 'react-router-dom';
import { LoginMenu } from './api-authorization/LoginMenu';
import classnames from 'classnames';
import { Icon } from 'semantic-ui-react'
import './NavMenu.css';

export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true,
            activeTab: 'lancamentos'
        };
    }

    componentDidMount() {
        if (window.location.pathname === '/contas') {
            this.setState({ activeTab: 'contas' })
        } else {
            this.setState({ activeTab: 'lancamentos' })
        }
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    render() {
        console.log(window.location.pathname)

        return (
            <>
                <header>
                    <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                        <Container>
                            <NavbarBrand tag={Link} to="/">Finance Management</NavbarBrand>
                            <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
                                <ul className="navbar-nav flex-grow">
                                    <LoginMenu>
                                    </LoginMenu>
                                </ul>
                            </Collapse>
                        </Container>
                    </Navbar>
                </header>
                <div className='container container-tab-menu'>
                    <Nav tabs>
                        <NavItem>
                            <NavLink
                                tag={Link}
                                to='/'
                                style={this.state.activeTab === 'lancamentos' ? {color: 'green'} : {color: 'gray'}}
                                className={classnames({ active: this.state.activeTab === 'lancamentos' })}
                                onClick={() => this.setState({ activeTab: 'lancamentos' })}
                            >
                                <Icon name='dollar' />{' '}
                                Lançamentos
                            </NavLink>
                        </NavItem>
                        <NavItem>
                            <NavLink
                                tag={Link}
                                to='/contas'
                                style={this.state.activeTab === 'contas' ? { color: 'green' } : { color: 'gray' }}
                                className={classnames({ active: this.state.activeTab === 'contas' })}
                                onClick={() => this.setState({ activeTab: 'contas' })}
                            >
                                <Icon name='folder' />{' '}
                                Contas
                            </NavLink>
                        </NavItem>
                    </Nav>
                </div>
            </>
        );
    }
}
