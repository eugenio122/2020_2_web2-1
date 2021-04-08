import './Contas.css'

import React, { useState, useEffect } from 'react';
import authService from '../api-authorization/AuthorizeService';
import { Card, CardBody, CardTitle, CardSubtitle, CardText, Button, Row, Col } from 'reactstrap';
import { Icon } from 'semantic-ui-react';
import { moneyLabel } from '../../helpers/FnUtils';
import ContaForm from './ContaForm';

export default function ContasList(props) {
    const [contas, setContas] = useState([]);
    const [loading, setLoading] = useState(false);
    const [user, setUser] = useState();

    const [showFormConta, setShowFormConta] = useState(false)
    const [contaEdit, setContaEdit] = useState(null)

    useEffect(() => {
        getContas();
    }, [])

    async function getContas() {
        const token = await authService.getAccessToken();
        const user = await authService.getUser()
        const response = await fetch('api/contas', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });

        const data = await response.json();

        setContas(data);
        setUser(user)
    }

    async function deleteConta(contaId) {
        if (!window.confirm('Tem certeza que deseja excluir essa conta?')) return null

        const token = await authService.getAccessToken();
        const response = await fetch(`api/contas/${contaId}`, {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` },
            method: 'DELETE'
        });

        getContas()
    }

    return (
        <div className='container-contas'>
            <Row style={{ width: '100%' }}>
                {contas && contas.length > 0 && contas.map(conta => (
                    <Col sm="6" style={{ marginBottom: 20 }} key={conta.id}>
                        <Card className='card-conta'>
                            <CardBody>
                                <CardTitle tag="h5">{conta.descConta}</CardTitle>
                                <CardSubtitle tag="h6" className={`${conta.saldo > 0 ? 'credit' : conta.saldo < 0 ? 'debit' : 'text-muted'}`}>{moneyLabel(conta.saldo)}</CardSubtitle>
                                <br />
                                <CardText>{conta.banco}</CardText>
                            </CardBody>
                            <CardBody>
                                <Button outline color='info' onClick={() => {
                                    setContaEdit(conta)
                                    setShowFormConta(true)
                                }}><Icon name='edit' /> Editar</Button>{" "}
                                {conta.descConta != 'Carteira Virtual' ? <Button outline color='danger' onClick={() => deleteConta(conta.id)}><Icon name='trash' /> Excluir</Button> : null}
                            </CardBody>
                        </Card>
                    </Col>

                ))}
                <Col sm="6" style={{ marginBottom: 20 }}>
                    <Card className='card-conta new-card' onClick={() => setShowFormConta(true)}>
                        <CardBody className='new-card-body'>
                            <CardText>+ nova conta</CardText>
                        </CardBody>
                    </Card>
                </Col>
            </Row>

            <ContaForm
                user={user}
                contaEdit={contaEdit}
                setContaEdit={setContaEdit}
                showFormConta={showFormConta}
                setShowFormConta={setShowFormConta}
                getContas={getContas} />
        </div>
    )
}